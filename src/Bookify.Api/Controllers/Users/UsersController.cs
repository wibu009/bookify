using Asp.Versioning;
using Bookify.Application.Users.GetLoggedInUser;
using Bookify.Application.Users.LoginUser;
using Bookify.Application.Users.RefreshToken;
using Bookify.Application.Users.RegisterUser;
using Bookify.Infrastructure.Authorization;
using Bookify.Shared.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Users;

[ApiController, ApiVersion(ApiVersions.V1), Route("api/v{version:apiVersion}/users"), AllowAnonymous]
public class UsersController(ISender sender) : ControllerBase
{
    [HttpGet("me"), HasPermission(Resources.Users, Actions.View)]
    public async Task<IActionResult> GetLoggedInUser(CancellationToken cancellationToken)
    {
        var query = new GetLoggedInUserQuery();
        var result = await sender.Send(query, cancellationToken);
        return Ok(result.Value);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password);
        var result = await sender.Send(command, cancellationToken);
        if (result.IsFailure) return BadRequest(result.Error);
        return Ok(result.Value);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(LogInUserRequest request, CancellationToken cancellationToken)
    {
        var command = new LogInUserCommand(
            request.Email,
            request.Password);
        var result = await sender.Send(command, cancellationToken);
        if (result.IsFailure) return Unauthorized(result.Error);
        return Ok(result.Value);
    }
    
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(string refreshToken, CancellationToken cancellationToken)
    {
        var command = new RefreshTokenCommand(refreshToken);
        var result = await sender.Send(command, cancellationToken);
        if (result.IsFailure) return Unauthorized(result.Error);
        return Ok(result.Value);
    }
}