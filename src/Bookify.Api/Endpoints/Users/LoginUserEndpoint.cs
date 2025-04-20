using Bookify.Application.Users;
using Bookify.Application.Users.LoginUser;
using MediatR;

namespace Bookify.Api.Endpoints.Users;

public class LoginUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("users/login", async (
                ISender sender,
                LogInUserRequest request,
                CancellationToken cancellationToken) =>
            {
                var command = new LogInUserCommand(request.Email, request.Password);
                var result = await sender.Send(command, cancellationToken);
                return result.IsFailure ? 
                    Results.Unauthorized() :
                    Results.Ok(result.Value);
            })
            .WithName("LoginUser")
            .WithSummary("Logs in a user and returns a JWT token.")
            .WithDescription("Allows a user to log in using email and password and receive a JWT token for authentication.")
            .Produces<TokenResponse>()
            .Produces(StatusCodes.Status401Unauthorized)
            .AllowAnonymous()
            .MapToApiVersion(1)
            .MapToApiVersion(2)
            .WithTags(Tags.Users);
    }
}

public sealed record LogInUserRequest(string Email, string Password);