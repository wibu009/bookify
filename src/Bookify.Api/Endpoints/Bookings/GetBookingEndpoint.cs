using Bookify.Api.Extensions;
using Bookify.Application.Bookings.GetBooking;
using Bookify.Shared.Authorization;
using MediatR;

namespace Bookify.Api.Endpoints.Bookings;

public class GetBookingEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet("bookings/{id:guid}", async (
                ISender sender, 
                Guid id, 
                CancellationToken cancellationToken) =>
            {
                var query = new GetBookingQuery(id);
                var result = await sender.Send(query, cancellationToken);
                return Results.Ok(result.Value);
            })
            .WithName("GetBooking")
            .WithSummary("Retrieves a booking by its unique ID.")
            .WithDescription("Allows users to retrieve details of a booking using its unique ID.")
            .Produces<BookingResponse>()
            .HasPermission(Resources.Bookings, Actions.View)
            .MapToApiVersion(1)
            .MapToApiVersion(2)
            .WithTags(Tags.Bookings);
    }
}