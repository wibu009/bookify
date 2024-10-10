using Bookify.Application.Abstractions.Messaging;

namespace Bookify.Application.Apartments.CreateApartment;

public record CreateApartmentCommand(
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
    ) : ICommand<Guid>;