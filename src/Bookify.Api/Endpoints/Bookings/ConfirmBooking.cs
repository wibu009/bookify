using Bookify.Api.Extensions;
using Bookify.Application.Bookings.ConfirmBooking;
using Bookify.Shared.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Endpoints.Bookings;

public class ConfirmBooking : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPut("bookings/{id:guid}/confirm", async (
                ISender sender,
                [FromRoute] Guid id,
                CancellationToken cancellationToken) => 
            {
                var command = new ConfirmBookingCommand(id);
                var result = await sender.Send(command, cancellationToken);
                return result.IsFailure ? Results.BadRequest(result.Error) : Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces<string>(StatusCodes.Status400BadRequest)
            .WithName("ConfirmBooking")
            .WithSummary("Confirms a booking.")
            .WithDescription("Allows users to confirm a booking using its unique ID.")
            .HasPermission(Resources.Bookings, Actions.Update)
            .MapToApiVersion(1)
            .MapToApiVersion(2)
            .WithTags(Tags.Bookings);
    }
}