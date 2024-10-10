using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Reviews;

public record Rating
{
    public static readonly Error Invalid = new("Rating.Invalid", "The rating is invalid");
    
    private Rating(int value) => Value = value;
    
    public int Value { get; private set; }
    
    public static Result<Rating> Create(int value)
    {
        return value is < 1 or > 5 ? Result.Failure<Rating>(Invalid) : new Rating(value);
    }
}