using System.Data;
using Bookify.Application.Abstractions.Persistent;
using Bookify.Domain.Abstractions;
using Bookify.Infrastructure.Jobs;
using Dapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;

namespace Bookify.Infrastructure.Outbox;

[DisallowConcurrentExecution, JobSchedule(0, 0 , 10)]
internal sealed class ProcessOutboxMessagesJob(
    ISqlConnectionFactory sqlConnectionFactory,
    IPublisher publisher,
    IOptions<OutboxOptions> outboxOptions,
    ILogger<ProcessOutboxMessagesJob> logger)
    : IJob
{
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    private readonly OutboxOptions _outboxOptions = outboxOptions.Value;

    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Beginning to process outbox messages...");

        using var connection = sqlConnectionFactory.CreateConnection();
        using var transaction = connection.BeginTransaction();
        
        var outboxMessages = await GetOutboxMessages(connection, transaction);
        foreach (var outboxMessage in outboxMessages)
        {
            Exception? exception = null;

            try
            {
                var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Content, JsonSerializerSettings)!;
                await publisher.Publish(domainEvent, context.CancellationToken);
            }
            catch (Exception caughtException)
            {
                logger.LogError(caughtException, "Exception while processing outbox message {MessageId}", outboxMessage.Id);
                exception = caughtException;
            }
            
            await UpdateOutboxMessageAsync(connection, transaction, outboxMessage, exception);
        }
        
        transaction.Commit();
        
        logger.LogInformation("Completed processing outbox messages");
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