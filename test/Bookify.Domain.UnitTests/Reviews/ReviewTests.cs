using Bookify.Domain.Bookings;
using Bookify.Domain.Reviews;
using Bookify.Domain.Reviews.Events;
using Bookify.Domain.Shared;
using Bookify.Domain.UnitTests.Apartments;
using Bookify.Domain.UnitTests.Infrastructure;
using Bookify.Domain.UnitTests.Users;
using Bookify.Domain.Users;

namespace Bookify.Domain.UnitTests.Reviews;

public class ReviewTests : BaseTest
{
    [Fact]
    public void Create_Should_SetReviewPropertyValues_WhenBookingIsCompleted()
    {
        // Arrange
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var price = new Money(10.0m, Currency.Usd);
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var period = DateRange.Create(today, today.AddDays(9));
        var apartment = ApartmentData.Create(price);
        var pricingService = new PricingService();
        var booking = Booking.Reserve(apartment, user.Id, period, DateTime.UtcNow, pricingService);
        booking.Confirm(DateTime.UtcNow);
        booking.Complete(DateTime.UtcNow);
        
        var rating = Rating.Create(5).Value;
        var comment = new Comment("Great stay!");
        var createdOnUtc = DateTime.UtcNow;

        // Act
        var result = Review.Create(booking, rating, comment, createdOnUtc);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.BookingId.Should().Be(booking.Id);
        result.Value.ApartmentId.Should().Be(booking.ApartmentId);
        result.Value.UserId.Should().Be(booking.UserId);
        result.Value.Rating.Should().Be(rating);
        result.Value.Comment.Should().Be(comment);
        result.Value.CreatedOnUtc.Should().Be(createdOnUtc);
    }
    
    [Fact]
    public void Create_Should_RaiseReviewCreatedDomainEvent_WhenBookingIsCompleted()
    {
        // Arrange
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var price = new Money(10.0m, Currency.Usd);
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var period = DateRange.Create(today, today.AddDays(9));
        var apartment = ApartmentData.Create(price);
        var pricingService = new PricingService();
        var booking = Booking.Reserve(apartment, user.Id, period, DateTime.UtcNow, pricingService);
        booking.Confirm(DateTime.UtcNow);
        booking.Complete(DateTime.UtcNow);
        
        var rating = Rating.Create(5).Value;
        var comment = new Comment("Great stay!");
        var createdOnUtc = DateTime.UtcNow;

        // Act
        var result = Review.Create(booking, rating, comment, createdOnUtc);

        // Assert
        var domainEvent = AssertDomainEventWasPublished<ReviewCreatedDomainEvent>(result.Value);
        domainEvent.ReviewId.Should().Be(result.Value.Id);
    }
    
    [Fact]
    public void Create_Should_ReturnFailure_WhenBookingIsNotCompleted()
    {
        // Arrange
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var price = new Money(10.0m, Currency.Usd);
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var period = DateRange.Create(today, today.AddDays(9));
        var apartment = ApartmentData.Create(price);
        var pricingService = new PricingService();
        var booking = Booking.Reserve(apartment, user.Id, period, DateTime.UtcNow, pricingService);

        var rating = Rating.Create(5).Value;
        var comment = new Comment("Great stay!");
        var createdOnUtc = DateTime.UtcNow;

        // Act
        var result = Review.Create(booking, rating, comment, createdOnUtc);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(ReviewErrors.NotEligible);
    }
}