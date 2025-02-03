using Bookify.Api.Extensions;
using Bookify.Application.Bookings.ReserveBooking;
using Bookify.Shared.Authorization;
using MediatR;

namespace Bookify.Api.Endpoints.Bookings;

public class ReserveBookingEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("bookings", async (
                ISender sender, 
                ReserveBookingRequest request, 
                CancellationToken cancellationToken) =>
            {
                var command = new ReserveBookingCommand(request.ApartmentId, request.UserId, request.StartDate, request.EndDate);
                var result = await sender.Send(command, cancellationToken);
                return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
            })
            .WithName("ReserveBooking")
            .WithSummary("Reserves a booking for an apartment.")
            .WithDescription("Allows users to reserve an apartment for a specified date range.")
            .Produces<Guid>()
            .Produces<string>(StatusCodes.Status400BadRequest)
            .HasPermission(Resources.Bookings, Actions.Create)
            .MapToApiVersion(1)
            .WithTags(Tags.Bookings);
    }
}

public sealed record ReserveBookingRequest(
    Guid ApartmentId,
    Guid UserId,
    DateOnly StartDate,
    DateOnly EndDate);