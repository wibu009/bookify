﻿using Bookify.Api.Extensions;
using Bookify.Application.Bookings.CompleteBooking;
using Bookify.Shared.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Endpoints.Bookings;

public class CompleteBookingEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPut("bookings/{id:guid}/complete", async (
                ISender sender,
                [FromRoute] Guid id,
                CancellationToken cancellationToken) => 
            {
                var command = new CompleteBookingCommand(id);
                var result = await sender.Send(command, cancellationToken);
                return result.IsFailure ? Results.BadRequest(result.Error) : Results.NoContent();
            })
            .Produces(StatusCodes.Status204NoContent)
            .Produces<string>(StatusCodes.Status400BadRequest)
            .WithName("CompleteBooking")
            .WithSummary("Marks a booking as complete.")
            .WithDescription("Allows users to mark a booking as complete using its unique ID.")
            .HasPermission(Resources.Bookings, Actions.Update)
            .MapToApiVersion(1)
            .MapToApiVersion(2)
            .WithTags(Tags.Bookings);
    }
}