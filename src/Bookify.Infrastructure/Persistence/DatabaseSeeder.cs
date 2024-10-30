using Bogus;
using Bookify.Domain.Apartments;
using Bookify.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace Bookify.Infrastructure.Persistence;

public sealed class DatabaseSeeder(ApplicationDbContext dbContext)
{
    public async Task SeedDataAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.Database.EnsureCreatedAsync(cancellationToken);
        var faker = new Faker();

        #region Apartments

        if (await dbContext.Set<Apartment>().AnyAsync(cancellationToken))
        {
            return;
        }
        var apartments = new List<Apartment>();

        for (var i = 0; i < 100; i++)
        {
            var apartment = Apartment.Create(
                name: new Name(faker.Company.CompanyName()),
                description: new Description("Amazing view"),
                address: new Address(
                    Country: faker.Address.Country(),
                    State: faker.Address.State(),
                    ZipCode: faker.Address.ZipCode(),
                    City: faker.Address.City(),
                    Street: faker.Address.StreetAddress()
                ),
                price: new Money(faker.Random.Decimal(50, 1000), Currency.Usd),
                cleaningFee: new Money(faker.Random.Decimal(25, 200), Currency.Usd),
                amenities: [Amenity.Parking, Amenity.MountainView]
            );

            apartments.Add(apartment);
        }
        
        dbContext.Set<Apartment>().AddRange(apartments);

        #endregion
        
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}