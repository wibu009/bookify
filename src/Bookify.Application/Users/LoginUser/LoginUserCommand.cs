using Bookify.Application.Abstractions.Messaging;

namespace Bookify.Application.Users.LoginUser;

public sealed record LoginUserCommand(string Email, string Password) : ICommand<AccessTokenResponse>;