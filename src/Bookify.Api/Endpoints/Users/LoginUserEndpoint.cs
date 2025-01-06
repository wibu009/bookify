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
        .WithDescription("Log in a user and return a JWT token.")
        .Produces<TokenResponse>()
        .AllowAnonymous()
        .MapToApiVersion(1)
        .WithTags(Tags.Users);
    }
}

public sealed record LogInUserRequest(string Email, string Password);