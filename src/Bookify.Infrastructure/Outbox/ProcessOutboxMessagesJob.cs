using System.Data;
using Bookify.Application.Abstractions.Persistent;
using Bookify.Domain.Abstractions;
using Bookify.Infrastructure.Jobs;
using Bookify.Infrastructure.Persistence;
using Dapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Quartz;

namespace Bookify.Infrastructure.Outbox;

[DisallowConcurrentExecution, JobSchedule(0, 0 , 10)]
internal sealed class ProcessOutboxMessagesJob(
    IPublisher publisher,
    IOptions<OutboxOptions> outboxOptions,
    ILogger<ProcessOutboxMessagesJob> logger,
    ApplicationDbContext dbContext) : IJob
{
    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    private readonly OutboxOptions _outboxOptions = outboxOptions.Value;

    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("Beginning to process outbox messages...");

        // Start a transaction using EF Core
        await using var transaction = await dbContext.Database.BeginTransactionAsync(context.CancellationToken);

        try
        {
            var outboxMessages = await GetOutboxMessages(dbContext);
            
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
                
                await UpdateOutboxMessageAsync(dbContext, outboxMessage, exception);
            }

            // Commit the transaction
            await transaction.CommitAsync(context.CancellationToken);
            logger.LogInformation("Completed processing outbox messages");
        }
        catch (Exception ex)
        {
            // Handle any errors that might occur during processing
            logger.LogError(ex, "An error occurred while processing outbox messages");
            await transaction.RollbackAsync(context.CancellationToken);
        }
    }

    private async Task<List<OutboxMessage>> GetOutboxMessages(ApplicationDbContext dbContext)
    {
        return await dbContext.Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null)
            .OrderBy(m => m.OccurredOnUtc)
            .Take(_outboxOptions.BatchSize)
            .ToListAsync();
    }

    private static async Task UpdateOutboxMessageAsync(ApplicationDbContext dbContext, OutboxMessage outboxMessage, Exception? exception)
    {
        outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
        outboxMessage.Error = exception?.Message;

        dbContext.Set<OutboxMessage>().Update(outboxMessage);
        await dbContext.SaveChangesAsync();
    }
}