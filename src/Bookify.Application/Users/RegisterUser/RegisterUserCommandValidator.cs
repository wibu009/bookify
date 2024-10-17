using FluentValidation;

namespace Bookify.Application.Users.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(u => u.FirstName).NotEmpty();
        RuleFor(u => u.LastName).NotEmpty();
        RuleFor(u => u.Email).EmailAddress();
        RuleFor(u => u.Password).MinimumLength(5);
    }
}