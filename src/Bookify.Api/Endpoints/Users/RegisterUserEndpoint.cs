using Bookify.Application.Users.RegisterUser;
using MediatR;

namespace Bookify.Api.Endpoints.Users;

public class RegisterUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("users", async (
                ISender sender,
                RegisterUserRequest request,
                CancellationToken cancellationToken) =>
            {
                var command = new RegisterUserCommand(
                    request.Email,
                    request.FirstName,
                    request.LastName,
                    request.Password);
                var result = await sender.Send(command, cancellationToken);
                return result.IsFailure ? 
                    Results.BadRequest(result.Error) : 
                    Results.Ok(result.Value);
            })
            .WithName("RegisterUser")
            .WithSummary("Registers a new user.")
            .WithDescription("Allows the creation of a new user with email, first name, last name, and password.")
            .Produces<Guid>(StatusCodes.Status200OK)
            .Produces<string>(StatusCodes.Status400BadRequest)
            .AllowAnonymous()
            .MapToApiVersion(1)
            .MapToApiVersion(2)
            .WithTags(Tags.Users);
    }
}

public sealed record RegisterUserRequest(string Email, string FirstName, string LastName, string Password);