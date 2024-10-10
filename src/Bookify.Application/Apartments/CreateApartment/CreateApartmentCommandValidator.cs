using Bookify.Domain.Apartments;
using Bookify.Domain.Shared;
using FluentValidation;

namespace Bookify.Application.Apartments.CreateApartment;

public class CreateApartmentCommandValidator : AbstractValidator<CreateApartmentCommand>
{
    public CreateApartmentCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");
        
        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero");
        
        RuleFor(x => x.CleaningFee)
            .GreaterThanOrEqualTo(0).WithMessage("Cleaning fee must be a positive value");
        
        RuleFor(x => x.Currency)
            .Must(BeAValidCurrency).WithMessage("Invalid currency code");

        RuleFor(x => x.Amenities)
            .Must(BeValidAmenities).WithMessage("One or more amenities are invalid");
    }

    private static bool BeAValidCurrency(string currencyCode)
    {
        try
        {
            Currency.FromCode(currencyCode);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static bool BeValidAmenities(List<int> amenities)
    {
        return amenities.All(amenity => Enum.IsDefined(typeof(Amenity), amenity));
    }
}
