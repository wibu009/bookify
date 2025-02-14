﻿using Bookify.Domain.Apartments;
using Bookify.Domain.Shared;

namespace Bookify.Domain.UnitTests.Apartments;

internal static class ApartmentData
{
    public static Guid Id => Guid.NewGuid();
    public static Name Name => new("Test Apartment");
    public static Description Description => new("Test Description");
    public static Address Address => new("Country", "State", "ZipCode", "City", "Street");
    public static Money Price => new(10.0m, Currency.Usd);
    public static Money CleaningFee => Money.Zero();
    public static List<Amenity> Amenities => [];
    public static Apartment Create(Money? price = null, Money? cleaningFee = null) => new(
        Id,
        Name,
        Description,
        Address,
        price ?? Price,
        cleaningFee ?? CleaningFee,
        Amenities
    );
}