using Bookify.Domain.Bookings;
using Bookify.Domain.Bookings.Events;
using Bookify.Domain.Shared;
using Bookify.Domain.UnitTests.Apartments;
using Bookify.Domain.UnitTests.Infrastructure;
using Bookify.Domain.UnitTests.Users;
using Bookify.Domain.Users;

namespace Bookify.Domain.UnitTests.Bookings;

public class BookingTests : BaseTest
{
    [Fact]
    public void Reserve_Should_RaiseBookingReservedDomainEvent()
    {
        // Arrange
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var price = new Money(10.0m, Currency.Usd);
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var period = DateRange.Create(today, today.AddDays(9)); // 10-day period starting today
        var apartment = ApartmentData.Create(price);
        var pricingService = new PricingService();
        
        // Act
        var booking = Booking.Reserve(apartment, user.Id, period, DateTime.UtcNow, pricingService);
        
        // Assert
        var domainEvent = AssertDomainEventWasPublished<BookingReservedDomainEvent>(booking);
        domainEvent.BookingId.Should().Be(booking.Id);
    }
    
    [Fact]
    public void Confirm_Should_ChangeStatusToConfirmed_AndRaiseBookingConfirmedDomainEvent()
    {
        // Arrange
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var price = new Money(10.0m, Currency.Usd);
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var period = DateRange.Create(today, today.AddDays(9));
        var apartment = ApartmentData.Create(price);
        var pricingService = new PricingService();
        var booking = Booking.Reserve(apartment, user.Id, period, DateTime.UtcNow, pricingService);

        // Act
        var result = booking.Confirm(DateTime.UtcNow);

        // Assert
        result.IsSuccess.Should().BeTrue();
        booking.Status.Should().Be(BookingStatus.Confirmed);
        booking.ConfirmedOnUtc.Should().NotBeNull();
        var domainEvent = AssertDomainEventWasPublished<BookingConfirmedDomainEvent>(booking);
        domainEvent.BookingId.Should().Be(booking.Id);
    }

    [Fact]
    public void Confirm_Should_ReturnFailure_WhenStatusIsNotReserved()
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

        // Act
        var result = booking.Confirm(DateTime.UtcNow);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BookingErrors.NotReserved);
    }

    [Fact]
    public void Reject_Should_ChangeStatusToRejected_AndRaiseBookingRejectedDomainEvent()
    {
        // Arrange
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var price = new Money(10.0m, Currency.Usd);
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var period = DateRange.Create(today, today.AddDays(9));
        var apartment = ApartmentData.Create(price);
        var pricingService = new PricingService();
        var booking = Booking.Reserve(apartment, user.Id, period, DateTime.UtcNow, pricingService);

        // Act
        var result = booking.Reject(DateTime.UtcNow);

        // Assert
        result.IsSuccess.Should().BeTrue();
        booking.Status.Should().Be(BookingStatus.Rejected);
        booking.RejectedOnUtc.Should().NotBeNull();
        var domainEvent = AssertDomainEventWasPublished<BookingRejectedDomainEvent>(booking);
        domainEvent.BookingId.Should().Be(booking.Id);
    }

    [Fact]
    public void Reject_Should_ReturnFailure_WhenStatusIsNotReserved()
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

        // Act
        var result = booking.Reject(DateTime.UtcNow);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BookingErrors.NotReserved);
    }

    [Fact]
    public void Complete_Should_ChangeStatusToCompleted_AndRaiseBookingCompletedDomainEvent()
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

        // Act
        var result = booking.Complete(DateTime.UtcNow);

        // Assert
        result.IsSuccess.Should().BeTrue();
        booking.Status.Should().Be(BookingStatus.Completed);
        booking.CompletedOnUtc.Should().NotBeNull();
        var domainEvent = AssertDomainEventWasPublished<BookingCompletedDomainEvent>(booking);
        domainEvent.BookingId.Should().Be(booking.Id);
    }

    [Fact]
    public void Complete_Should_ReturnFailure_WhenStatusIsNotConfirmed()
    {
        // Arrange
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var price = new Money(10.0m, Currency.Usd);
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var period = DateRange.Create(today, today.AddDays(9));
        var apartment = ApartmentData.Create(price);
        var pricingService = new PricingService();
        var booking = Booking.Reserve(apartment, user.Id, period, DateTime.UtcNow, pricingService);

        // Act
        var result = booking.Complete(DateTime.UtcNow);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BookingErrors.NotConfirmed);
    }

    [Fact]
    public void Cancel_Should_ChangeStatusToCancelled_AndRaiseBookingCancelledDomainEvent()
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

        // Act
        var result = booking.Cancel(DateTime.UtcNow);

        // Assert
        result.IsSuccess.Should().BeTrue();
        booking.Status.Should().Be(BookingStatus.Cancelled);
        booking.CancelledOnUtc.Should().NotBeNull();
        var domainEvent = AssertDomainEventWasPublished<BookingCancelledDomainEvent>(booking);
        domainEvent.BookingId.Should().Be(booking.Id);
    }

    [Fact]
    public void Cancel_Should_ReturnFailure_WhenBookingHasAlreadyStarted()
    {
        // Arrange
        var user = User.Create(UserData.FirstName, UserData.LastName, UserData.Email);
        var price = new Money(10.0m, Currency.Usd);
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var period = DateRange.Create(today.AddDays(-1), today.AddDays(9)); // Start date in the past
        var apartment = ApartmentData.Create(price);
        var pricingService = new PricingService();
        var booking = Booking.Reserve(apartment, user.Id, period, DateTime.UtcNow, pricingService);
        booking.Confirm(DateTime.UtcNow);

        // Act
        var result = booking.Cancel(DateTime.UtcNow);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BookingErrors.AlreadyStarted);
    }
}