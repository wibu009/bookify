namespace Bookify.Domain.Shared;

public record Currency
{
    internal static readonly Currency None = new("");
    public static readonly Currency Usd = new("USD");
    public static readonly Currency Eur = new("EUR");

    private Currency(string code) => Code = code;
    
    public string Code { get; init; }
    public Currency FromCode(string code) 
        => All.FirstOrDefault(x => x.Code == code) 
                                             ?? throw new ApplicationException("Invalid currency code");
    
    public static readonly IReadOnlyCollection<Currency> All =
    [
        Usd, Eur
    ];
}