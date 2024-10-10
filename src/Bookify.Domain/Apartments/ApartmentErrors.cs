using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Apartments;

public static class ApartmentErrors
{
    public static readonly Error NotFound = new("Apartment.NotFound", "The apartment with the specified identifier was not found");
    public static readonly Error Invalid = new("Apartment.Invalid", "The apartment with the specified identifier is invalid");
    public static readonly Error AmenityAlreadyAdded = new("Apartment.AmenityAlreadyAdded", "The apartment with the specified identifier already has the specified amenity");
}