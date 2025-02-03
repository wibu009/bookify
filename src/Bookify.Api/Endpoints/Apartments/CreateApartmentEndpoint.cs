using Bookify.Api.Extensions;
using Bookify.Application.Apartments.CreateApartment;
using Bookify.Shared.Authorization;
using MediatR;

namespace Bookify.Api.Endpoints.Apartments;

public class CreateApartmentEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost("apartments", async (
                ISender sender, 
                CreateApartmentRequest request, 
                CancellationToken cancellationToken) =>
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
                return result.IsFailure ? Results.BadRequest(result.Error) : Results.Ok(result.Value);
            })
            .WithName("CreateApartment")
            .WithSummary("Creates a new apartment listing.")
            .WithDescription("Allows authorized users to create a new apartment by providing details like name, location, price, and amenities.")
            .Produces<Guid>(StatusCodes.Status200OK)
            .Produces<string>(StatusCodes.Status400BadRequest)
            .HasPermission(Resources.Apartments, Actions.Create)
            .MapToApiVersion(1)
            .WithTags(Tags.Apartments);
    }
}

public sealed record CreateApartmentRequest(
    string Name,
    string Description,
    string Country,
    string State,
    string ZipCode,
    string City,
    string Street,
    decimal Price,
    decimal CleaningFee,
    string Currency,
    List<int> Amenities
);