using Bookify.Application.Abstractions.Authentication;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Users;
using Bookify.Shared.Core;

namespace Bookify.Application.Users.LoginUser;

internal sealed class LogInUserCommandHandler(IJwtService jwtService)
    : ICommandHandler<LogInUserCommand, TokenResponse>
{
    public async Task<Result<TokenResponse>> Handle(LogInUserCommand request, CancellationToken cancellationToken)
    {
        var result = await jwtService.GetTokenAsync(request.Email, request.Password, cancellationToken);
        
        return result.IsFailure 
            ? Result.Failure<TokenResponse>(UserErrors.InvalidCredentials) 
            : result.Value;
    }
}