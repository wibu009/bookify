using Bookify.Application.Abstractions.Messaging;
using Bookify.Application.Users.LoginUser;

namespace Bookify.Application.Users.RefreshToken;

public sealed record RefreshTokenCommand(
    string RefreshToken) : ICommand<TokenResponse>;