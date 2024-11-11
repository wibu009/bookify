using Bookify.Application.Abstractions.Authentication;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Users.LoginUser;
using Bookify.Domain.Users;
using Bookify.Shared.Core;

namespace Bookify.Application.Users.RefreshToken;

internal sealed class RefreshTokenCommandHandler(IJwtService jwtService)
    : ICommandHandler<RefreshTokenCommand, TokenResponse>
{
    public async Task<Result<TokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var result = await jwtService.RefreshTokenAsync(request.RefreshToken, cancellationToken);
        
        return result.IsFailure 
            ? Result.Failure<TokenResponse>(UserErrors.InvalidCredentials) 
            : result.Value;
    }
}