using Bookify.Application.Abstractions.Messaging;

namespace Bookify.Application.Abstractions.Caching;

public interface ICacheQuery<TResponse> : IQuery<TResponse>, ICacheQuery;

public interface ICacheQuery
{
    string CacheKey { get; }
    
    TimeSpan? Expiration { get; }
}