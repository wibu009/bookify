using Bookify.Api.Extensions;
using Bookify.Application.Apartments.SearchApartments;
using Bookify.Shared.Authorization;
using MediatR;

namespace Bookify.Api.Endpoints.Apartments;

public class SearchApartmentEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("apartments/search", async (
            ISender sender,
            DateOnly startDate,
            DateOnly endDate,
            CancellationToken cancellationToken) =>
        {
            var query = new SearchApartmentsQuery(startDate, endDate);
            var result = await sender.Send(query, cancellationToken);
            return Results.Ok(result.Value);
        })
        .WithName("SearchApartment")
        .WithDescription("Search for apartments based on start and end dates of the booking.")
        .Produces<List<ApartmentResponse>>()
        .HasPermission(Resources.Apartments, Actions.Search)
        .MapToApiVersion(1)
        .WithTags(Tags.Apartments);
    }
}