using Asp.Versioning;
using Bookify.Application.Apartments.CreateApartment;
using Bookify.Application.Apartments.SearchApartments;
using Bookify.Infrastructure.Authorization;
using Bookify.Shared.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Apartments;

[ApiController, ApiVersion(ApiVersions.V1), Route("api/v{version:apiVersion}/apartments"), Authorize]
public class ApartmentsController(ISender sender) : ControllerBase
{
    [HttpGet("search"), HasPermission(Resources.Apartments, Actions.Search)]
    public async Task<IActionResult> SearchApartments(
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken)
    {
        var query = new SearchApartmentsQuery(startDate, endDate);
        var result = await sender.Send(query, cancellationToken);
        return Ok(result.Value);
    }
    
    [HttpPost, HasPermission(Resources.Apartments, Actions.Create)]
    public async Task<IActionResult> CreateApartment(CreateApartmentRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateApartmentCommand(
            request.Name,
            request.Description,
            request.Country,
            request.State,
            request.ZipCode,
            request.City,
            request.Street,
            request.Price,
            request.CleaningFee,
            request.Currency,
            request.Amenities);
        var result = await sender.Send(command, cancellationToken);
        return result.IsFailure
            ? BadRequest(result.Error)
            : Ok(result.Value);
    }
}