namespace Bookify.Api.Endpoints;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder endpointRouteBuilder);
}