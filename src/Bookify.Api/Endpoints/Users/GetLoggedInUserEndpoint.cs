using Bookify.Api.Extensions;
using Bookify.Application.Users.GetLoggedInUser;
using Bookify.Shared.Authorization;
using MediatR;

namespace Bookify.Api.Endpoints.Users;

public class GetLoggedInUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("users/me", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetLoggedInUserQuery();
            var result = await sender.Send(query, cancellationToken);
            return Results.Ok(result.Value);
        })
        .WithName("GetLoggedInUser")
        .WithDescription("Get the currently logged in user.")
        .Produces<UserResponse>()
        .HasPermission(Resources.Users, Actions.View)
        .MapToApiVersion(1)
        .WithTags(Tags.Users);
    }
}