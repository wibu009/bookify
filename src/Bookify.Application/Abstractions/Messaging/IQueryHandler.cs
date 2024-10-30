using Bookify.Domain.Abstractions;
using Bookify.Shared.Core;
using MediatR;

namespace Bookify.Application.Abstractions.Messaging;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> 
    where TQuery : IQuery<TResponse>;