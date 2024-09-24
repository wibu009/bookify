namespace Bookify.Application.Apartments.SearchApartments;

public sealed class AddressResponse
{
    public string Country { get; init; } = default!;
    public string State { get; init; } = default!;
    public string ZipCode { get; init; } = default!;
    public string City { get; init; } = default!;
    public string Street { get; init; } = default!;
}