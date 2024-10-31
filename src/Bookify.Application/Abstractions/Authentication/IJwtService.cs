using Bookify.Application.Users.LoginUser;
using Bookify.Shared.Core;

namespace Bookify.Application.Abstractions.Authentication;

public interface IJwtService
{
    Task<Result<TokenResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<Result<TokenResponse>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
}