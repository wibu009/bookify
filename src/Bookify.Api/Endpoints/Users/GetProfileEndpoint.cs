using Bookify.Api.Extensions;
using Bookify.Application.Users.GetLoggedInUser;
using Bookify.Shared.Authorization;
using MediatR;

namespace Bookify.Api.Endpoints.Users;

public class GetProfileEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users/me", async (ISender sender, CancellationToken cancellationToken) =>
        {
            var query = new GetLoggedInUserQuery();
            var result = await sender.Send(query, cancellationToken);
            return Results.Ok(result.Value);
        })
        .HasPermission(Resources.Users, Actions.View)
        .MapToApiVersion(ApiVersions.V1)
        .WithTags(Tags.Users);
    }
}