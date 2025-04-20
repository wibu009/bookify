using Bookify.Application.Users;
using Bookify.Application.Users.RefreshToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Endpoints.Users;

public class RefreshTokenEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("users/refresh-token", async (
                ISender sender,
                [FromBody] string refreshToken,
                CancellationToken cancellationToken) =>
            {
                var command = new RefreshTokenCommand(refreshToken);
                var result = await sender.Send(command, cancellationToken);
                return result.IsFailure ? 
                    Results.Unauthorized() :
                    Results.Ok(result.Value);
            })
            .WithName("RefreshToken")
            .WithSummary("Refreshes a user's token and returns new access and refresh tokens.")
            .WithDescription("Allows a user to refresh their access token using a valid refresh token.")
            .Produces<TokenResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status401Unauthorized)
            .AllowAnonymous()
            .MapToApiVersion(1)
            .MapToApiVersion(2)
            .WithTags(Tags.Users);
    }
}