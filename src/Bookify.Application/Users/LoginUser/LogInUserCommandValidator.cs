using FluentValidation;

namespace Bookify.Application.Users.LoginUser;

public class LogInUserCommandValidator : AbstractValidator<LogInUserCommand>
{
    public LogInUserCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}