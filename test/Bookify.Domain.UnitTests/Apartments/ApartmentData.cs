using Bookify.Domain.Apartments;
using Bookify.Domain.Shared;

namespace Bookify.Domain.UnitTests.Apartments;

public static class ApartmentData
{
    public static Name Name => new("Test Apartment");
    public static Description Description => new("Test Description");
    public static Address Address => new("Country", "State", "ZipCode", "City", "Street");
    public static Money Price => new(10.0m, Currency.Usd);
    public static Money CleaningFee => Money.Zero();
    public static List<Amenity> Amenities => [];
    public static Apartment Create(Money? price = null, Money? cleaningFee = null) => Apartment.Create(
        Name,
        Description,
        Address,
        price ?? Price,
        cleaningFee ?? CleaningFee,
        Amenities
    ).Value;
}