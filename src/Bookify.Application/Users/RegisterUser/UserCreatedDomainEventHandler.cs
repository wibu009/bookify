using Bookify.Application.Abstractions.Email;
using Bookify.Domain.Users;
using Bookify.Domain.Users.Events;
using MediatR;

namespace Bookify.Application.Users.RegisterUser;

public class UserCreatedDomainEventHandler(
    IUserRepository repository,
    IEmailService emailService) 
    : INotificationHandler<UserCreatedDomainEvent>
{
    public async Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var user = await repository.GetByIdAsync(notification.UserId, cancellationToken);
        if (user is null)
        {
            return;
        }
        
        await emailService
            .SendAsync(
                [user.Email.Value],
                "Welcome!",
                "You have successfully registered!",
                ct: cancellationToken);
    }
}