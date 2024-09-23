using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Bookings.Events;
using Bookify.Domain.Shared;

namespace Bookify.Domain.Bookings;

public sealed class Booking : Entity
{
    private Booking(
        Guid id,
        Guid apartmentId,
        Guid userId,
        DateRange duration,
        Money priceForPeriod,
        Money cleaningFee,
        Money amenitiesUpCharge,
        Money totalPrice,
        BookingStatus status,
        DateTime createdOnUtc
        ) : base(id)
    {
        ApartmentId = apartmentId;
        UserId = userId;
        Duration = duration;
        PriceForPeriod = priceForPeriod;
        CleaningFee = cleaningFee;
        AmenitiesUpCharge = amenitiesUpCharge;
        TotalPrice = totalPrice;
        Status = status;
        CreatedOnUtc = createdOnUtc;
    }
    
    public Guid ApartmentId { get; private set; }
    public Guid UserId { get; private set; }
    public DateRange Duration { get; private set; }
    public Money PriceForPeriod { get; private set; }
    public Money CleaningFee { get; private set; }
    public Money AmenitiesUpCharge { get; private set; }
    public Money TotalPrice { get; private set; }
    public BookingStatus Status { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? ConfirmedOnUtc { get; private set; }
    public DateTime? RejectedOnUtc { get; private set; }
    public DateTime? CompletedOnUtc { get; private set; }
    public DateTime? CancelledOnUtc { get; private set; }

    public static Booking Reserve(
        Apartment apartment,
        Guid userId,
        DateRange duration,
        DateTime utcNow,
        PricingService pricingService)
    {
        var pricingDetails = pricingService.CalculatePrice(apartment, duration);
        
        var booking = new Booking(
            Guid.NewGuid(),
            apartment.Id,
            userId,
            duration,
            pricingDetails.PriceForPeriod,
            pricingDetails.CleaningFee,
            pricingDetails.AmenitiesUpCharge,
            pricingDetails.TotalPrice,
            BookingStatus.Reserved,
            utcNow
        );
        
        booking.RaiseDomainEvent(new BookingReservedDomainEvent(booking.Id));
        
        apartment.LastBookedOnUtc = utcNow;
        
        return booking;
    }
    
    public Result Confirm(DateTime utcNow)
    {
        if (Status != BookingStatus.Reserved)
        {
            return Result.Failure(BookingErrors.NotReserved);
        }
        
        ConfirmedOnUtc = utcNow;
        Status = BookingStatus.Confirmed;
        
        RaiseDomainEvent(new BookingConfirmedDomainEvent(Id));
        
        return Result.Success();
    }
    
    public Result Reject(DateTime utcNow)
    {
        if (Status != BookingStatus.Reserved)
        {
            return Result.Failure(BookingErrors.NotReserved);
        }
        
        RejectedOnUtc = utcNow;
        Status = BookingStatus.Rejected;
        
        RaiseDomainEvent(new BookingRejectedDomainEvent(Id));
        
        return Result.Success();
    }
    
    public Result Complete(DateTime utcNow)
    {
        if (Status != BookingStatus.Confirmed)
        {
            return Result.Failure(BookingErrors.NotConfirmed);
        }
        
        CompletedOnUtc = utcNow;
        Status = BookingStatus.Completed;
        
        RaiseDomainEvent(new BookingCompletedDomainEvent(Id));
        
        return Result.Success();
    }
    
    public Result Cancel(DateTime utcNow)
    {
        if (Status != BookingStatus.Confirmed)
        {
            return Result.Failure(BookingErrors.NotConfirmed);
        }
        
        var currentDate = DateOnly.FromDateTime(utcNow);
        if (Duration.Start < currentDate)
        {
            return Result.Failure(BookingErrors.AlreadyStarted);
        }
        
        CancelledOnUtc = utcNow;
        Status = BookingStatus.Cancelled;
        
        RaiseDomainEvent(new BookingCancelledDomainEvent(Id));
        
        return Result.Success();
    }
}