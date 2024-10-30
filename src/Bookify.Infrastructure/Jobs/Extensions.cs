using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Exception = System.Exception;

namespace Bookify.Infrastructure.Jobs;

public static class Extensions
{
    public static IServiceCollection AddJobs(this IServiceCollection services)
    {
        var jobTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => typeof(IJob).IsAssignableFrom(t) && !t.IsAbstract);

        foreach (var jobType in jobTypes)
        {
            services.AddTransient(jobType);
            
            var jobSchedule = jobType.GetCustomAttribute<JobScheduleAttribute>();
            if (jobSchedule != null)
            {
                services.AddQuartz(configure =>
                {
                    var jobKey = new JobKey(jobType.Name);
                    configure.AddJob(jobType, jobKey);
                    
                    if (!string.IsNullOrWhiteSpace(jobSchedule.CronExpression))
                    {
                        configure.AddTrigger(opts => opts
                            .ForJob(jobKey)
                            .WithIdentity($"{jobType.Name}-trigger")
                            .StartNow()
                            .WithCronSchedule(jobSchedule.CronExpression)
                        );
                    }
                    else if (jobSchedule.Interval.HasValue)
                    {
                        var simpleScheduleBuilder =
                            SimpleScheduleBuilder.Create()
                                .WithInterval(jobSchedule.Interval.Value);
                        
                        simpleScheduleBuilder = jobSchedule.RepeatCount.HasValue
                            ? simpleScheduleBuilder.WithRepeatCount(jobSchedule.RepeatCount.Value)
                            : simpleScheduleBuilder.RepeatForever();

                        configure.AddTrigger(opts => opts
                            .ForJob(jobKey)
                            .WithIdentity($"{jobType.Name}-trigger")
                            .StartNow()
                            .WithSimpleSchedule(simpleScheduleBuilder)
                        );
                    }
                    else
                    {
                        throw new Exception($"Job {jobType.Name} must have either CronExpression or Interval set");
                    }
                });
            }
        }

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        } );
        
        return services;
    }
}