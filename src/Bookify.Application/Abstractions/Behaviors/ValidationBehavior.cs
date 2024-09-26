using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Exceptions;
using FluentValidation;
using MediatR;
using ValidationException = FluentValidation.ValidationException;

namespace Bookify.Application.Abstractions.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseCommand
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (validators.Any())
        {
            return await next();
        }
        
        var context = new ValidationContext<TRequest>(request);
        var validationErrors = validators
            .Select(v => v.Validate(context))
            .Where(vr => vr.Errors.Count != 0)
            .SelectMany(vr => vr.Errors)
            .Select(vf => new ValidationError(vf.PropertyName, vf.ErrorMessage))
            .ToList();
        
        if (validationErrors.Any())
        {
            throw new Exceptions.ValidationException(validationErrors);
        }
        
        return await next();
    }
}