using Bookify.Application.Abstractions.Caching;
using Bookify.Domain.Abstractions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bookify.Application.Abstractions.Behaviors;

internal sealed class QueryCachingBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICacheQuery
    where TResponse : Result
{
    private readonly ICacheService _cacheService;
    private readonly ILogger<QueryCachingBehavior<TRequest, TResponse>> _logger;

    public QueryCachingBehavior(
        [FromKeyedServices("distributed")] ICacheService cacheService,
        ILogger<QueryCachingBehavior<TRequest, TResponse>> logger)
    {
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var name = request.GetType().Name;
        var cachedResult = await _cacheService.GetAsync<TResponse>(request.CacheKey, cancellationToken);
        if (cachedResult is not null)
        {
            _logger.LogInformation("Cache hit for {Query}", name);
            return cachedResult;
        }
        
        _logger.LogInformation("Cache miss for {Query}", name);
        var result = await next();
        if (result.IsSuccess)
            await _cacheService.SetAsync(request.CacheKey, result, request.Expiration, cancellationToken);
        
        return result;
    }
}