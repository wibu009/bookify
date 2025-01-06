using Bookify.Api.Extensions;
using Bookify.Application.Bookings.CancelBooking;
using Bookify.Shared.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Endpoints.Bookings;

public class CancelBookingEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPut("bookings/{id:guid}/cancel", async (
            ISender sender,
            [FromRoute] Guid id,
            CancellationToken cancellationToken) => 
        {
            var command = new CancelBookingCommand(id);
            var result = await sender.Send(command, cancellationToken);
            return result.IsFailure ? Results.BadRequest(result.Error) : Results.NoContent();
        })
        .Produces(StatusCodes.Status204NoContent)
        .WithName("CancelBooking")
        .WithDescription("Cancel a booking.")
        .HasPermission(Resources.Bookings, Actions.Update)
        .MapToApiVersion(1)
        .WithTags(Tags.Bookings);
    }
}