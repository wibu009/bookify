using Bookify.Domain.Apartments;
using Bookify.Domain.Apartments.Events;
using Bookify.Domain.UnitTests.Infrastructure;

namespace Bookify.Domain.UnitTests.Apartments;

public class ApartmentTests : BaseTest
{
    [Fact]
    public void Create_Should_SetPropertyValues()
    {
        // Act
        var apartment = Apartment.Create(
            ApartmentData.Name,
            ApartmentData.Description,
            ApartmentData.Address,
            ApartmentData.Price,
            ApartmentData.CleaningFee,
            ApartmentData.Amenities);

        // Assert
        apartment.Name.Should().Be(ApartmentData.Name);
        apartment.Description.Should().Be(ApartmentData.Description);
        apartment.Address.Should().Be(ApartmentData.Address);
        apartment.Price.Should().Be(ApartmentData.Price);
        apartment.CleaningFee.Should().Be(ApartmentData.CleaningFee);
        apartment.Amenities.Should().BeEquivalentTo(ApartmentData.Amenities);
    }

    [Fact]
    public void Create_Should_RaiseApartmentCreatedDomainEvent()
    {
        // Act
        var apartment = Apartment.Create(
            ApartmentData.Name,
            ApartmentData.Description,
            ApartmentData.Address,
            ApartmentData.Price,
            ApartmentData.CleaningFee,
            ApartmentData.Amenities);

        // Assert
        var domainEvent = AssertDomainEventWasPublished<ApartmentCreatedDomainEvent>(apartment);
        domainEvent.ApartmentId.Should().Be(apartment.Id);
    }
}
