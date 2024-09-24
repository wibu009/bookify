namespace Bookify.Application.Exceptions;

public record ValidationError(string PropertyName, string ErrorMessage);