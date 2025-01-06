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
        .WithDescription("Get a booking by id.")
        .Produces<BookingResponse>()
        .HasPermission(Resources.Bookings, Actions.View)
        .MapToApiVersion(1)
        .WithTags(Tags.Bookings);
    }
}