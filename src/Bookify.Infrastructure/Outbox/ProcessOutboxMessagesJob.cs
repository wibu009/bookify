using System.Data;
using Bookify.Application.Abstractions.Data;
using Bookify.Domain.Abstractions;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;

namespace Bookify.Infrastructure.Outbox;

[DisallowConcurrentExecution]
internal sealed class ProcessOutboxMessagesJob : IJob
{
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };
    
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IPublisher _publisher;
    private readonly OutboxOptions _outboxOptions;
    private readonly ILogger<ProcessOutboxMessagesJob> _logger;

    public ProcessOutboxMessagesJob(
        ISqlConnectionFactory sqlConnectionFactory,
        IPublisher publisher,
        IOptions<OutboxOptions> outboxOptions,
        ILogger<ProcessOutboxMessagesJob> logger)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _publisher = publisher;
        _outboxOptions = outboxOptions.Value;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Beginning to process outbox messages...");

        using var connection = _sqlConnectionFactory.CreateConnection();
        using var transaction = connection.BeginTransaction();
        
        var outboxMessages = await GetOutboxMessages(connection, transaction);
        foreach (var outboxMessage in outboxMessages)
        {
            Exception? exception = null;

            try
            {
                var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Content, JsonSerializerSettings)!;
                await _publisher.Publish(domainEvent, context.CancellationToken);
            }
            catch (Exception caughtException)
            {
                _logger.LogError(caughtException, "Exception while processing outbox message {MessageId}", outboxMessage.Id);
                exception = caughtException;
            }
            
            await UpdateOutboxMessageAsync(connection, transaction, outboxMessage, exception);
        }
        
        transaction.Commit();
        
        _logger.LogInformation("Completed processing outbox messages");
    }

    private async Task<IReadOnlyList<OutboxMessageResponse>> GetOutboxMessages(IDbConnection connection,
        IDbTransaction transaction)
    {
        const string sql = $"""
                            SELECT id, content
                            FROM outbox_messages
                            WHERE processed_on_utc IS NULL
                            ORDER BY occurred_on_utc
                            LIMIT @BatchSize
                            FOR UPDATE 
                            """;

        var outboxMessages = await connection.QueryAsync<OutboxMessageResponse>(sql,
            new { BatchSize = _outboxOptions.BatchSize }, transaction);
        
        return outboxMessages.ToList();
    }
    
    private static async Task UpdateOutboxMessageAsync(IDbConnection connection,
        IDbTransaction transaction, OutboxMessageResponse outboxMessage,
        Exception? exception)
    {
        const string sql = $"""
                            UPDATE outbox_messages
                            SET processed_on_utc = @ProcessedOnUtc,
                                error = @Error
                            WHERE id = @Id
                            """;

        await connection.ExecuteAsync(sql, new
        {
            Id = outboxMessage.Id,
            ProcessedOnUtc = DateTime.UtcNow,
            Error = exception?.Message
        }, transaction);
    }

    internal sealed record OutboxMessageResponse(Guid Id, string Content);
}