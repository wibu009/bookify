using Bookify.Application.Abstractions.Messaging;

namespace Bookify.Application.Users.RegisterUser;

public record RegisterUserCommand(
    string Email,
    string FirstName,
    string LastName,
    string Password) : ICommand<Guid>;