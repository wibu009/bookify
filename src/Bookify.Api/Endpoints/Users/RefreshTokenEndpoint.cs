using Bookify.Application.Users;
using Bookify.Application.Users.RefreshToken;
using MediatR;

namespace Bookify.Api.Endpoints.Users;

public class RefreshTokenEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("users/refresh-token", async (
            ISender sender,
            string refreshToken,
            CancellationToken cancellationToken) =>
        {
            var command = new RefreshTokenCommand(refreshToken);
            var result = await sender.Send(command, cancellationToken);
            return result.IsFailure ? 
                Results.Unauthorized() :
                Results.Ok(result.Value);
        })
        .WithName("RefreshToken")
        .WithDescription("Refresh a user's token and return a new access token and refresh token.")
        .Produces<TokenResponse>()
        .AllowAnonymous()
        .MapToApiVersion(1)
        .WithTags(Tags.Users);
    }
}