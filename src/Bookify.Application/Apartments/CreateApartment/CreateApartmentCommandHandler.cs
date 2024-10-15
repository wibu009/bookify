using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Apartments;
using Bookify.Domain.Shared;

namespace Bookify.Application.Apartments.CreateApartment;

internal sealed class CreateApartmentCommandHandler(
    IApartmentRepository apartmentRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateApartmentCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateApartmentCommand request, CancellationToken cancellationToken)
    {
        var address = new Address(
            request.Country,
            request.State,
            request.ZipCode,
            request.City,
            request.Street);
        var price = new Money(request.Price, Currency.FromCode(request.Currency));
        var cleaningFee = new Money(request.CleaningFee, Currency.FromCode(request.Currency));
        var amenities = request.Amenities.Select(amenity => (Amenity)amenity).ToList();
        var apartment = Apartment.Create(
            new Name(request.Name),
            new Description(request.Description),
            address,
            price,
            cleaningFee,
            amenities);
        
        apartmentRepository.Add(apartment);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return apartment.Id;
    }
}