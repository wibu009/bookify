﻿using Bookify.Application.Users.GetLoggedInUser;
using Bookify.Application.Users.LoginUser;
using Bookify.Application.Users.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookify.Api.Controllers.Users;

[ApiController, Route("api/users")]
public class UsersController(ISender sender) : ControllerBase
{
    [HttpGet("me"), Authorize(Roles = Roles.Registered)]
    public async Task<IActionResult> GetLoggedInUser(CancellationToken cancellationToken)
    {
        var query = new GetLoggedInUserQuery();
        var result = await sender.Send(query, cancellationToken);
        return Ok(result.Value);
    }
    
    [HttpPost("register"), AllowAnonymous]
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
    
    [HttpPost("login"), AllowAnonymous]
    public async Task<IActionResult> LoginUser(LoginUserRequest request, CancellationToken cancellationToken)
    {
        var command = new LoginUserCommand(
            request.Email,
            request.Password);
        var result = await sender.Send(command, cancellationToken);
        if (result.IsFailure) return Unauthorized(result.Error);
        return Ok(result.Value);
    }
}