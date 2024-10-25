namespace Bookify.Shared.Core;

public record ValidationError(string PropertyName, string ErrorMessage);