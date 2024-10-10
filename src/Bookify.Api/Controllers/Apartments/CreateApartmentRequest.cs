namespace Bookify.Api.Controllers.Apartments;

public record CreateApartmentRequest(
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