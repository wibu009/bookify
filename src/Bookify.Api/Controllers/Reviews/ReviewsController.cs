using Bookify.Application.Reviews.AddReview;
using Bookify.Infrastructure.Authorization;
using Bookify.Shared.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Reviews;

[ApiController, Route("api/v{version:apiVersion}/reviews"), Authorize]
public class ReviewsController(ISender sender) : ControllerBase
{
    [HttpPost, HasPermission(Resources.Reviews, Actions.Create)]
    public async Task<IActionResult> Create(CreateReviewRequest request, CancellationToken cancellationToken)
    {
        var command = new AddReviewCommand(request.BookingId, request.Rating, request.Comment);
        var result = await sender.Send(command, cancellationToken);
        return result.IsFailure
            ? BadRequest(result.Error)
            : Ok(result.Value);
    }
}