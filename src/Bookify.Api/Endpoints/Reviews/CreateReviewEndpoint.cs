using Bookify.Api.Extensions;
using Bookify.Application.Reviews.AddReview;
using Bookify.Shared.Authorization;
using MediatR;

namespace Bookify.Api.Endpoints.Reviews;

public class CreateReviewEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("reviews", async (
                ISender sender, 
                CreateReviewRequest request, 
                CancellationToken cancellationToken) =>
            {
                var command = new AddReviewCommand(request.BookingId, request.Rating, request.Comment);
                var result = await sender.Send(command, cancellationToken);
                return result.IsFailure ? 
                    Results.BadRequest(result.Error) :
                    Results.Ok(result.Value);
            })
            .WithName("CreateReview")
            .WithSummary("Creates a review for a booking.")
            .WithDescription("Allows users to create a review for an apartment based on their booking.")
            .Produces<Guid>()
            .Produces<string>(StatusCodes.Status400BadRequest)
            .HasPermission(Resources.Reviews, Actions.Create)
            .MapToApiVersion(1)
            .WithTags(Tags.Reviews);
    }
}

public sealed record CreateReviewRequest(
    Guid BookingId,
    int Rating,
    string Comment);