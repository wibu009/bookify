using Bookify.Domain.Abstractions;
using Bookify.Shared.Core;
using MediatR;

namespace Bookify.Application.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;