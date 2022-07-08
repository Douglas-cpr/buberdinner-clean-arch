using BuberDinner.Api.Filters;
using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Services.Authentication;
using BuberDinner.Contracts.Authentication;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

[ApiController]
[Route("auth")]
[ErrorHandlingFilter]
public class AuthenticationController : ControllerBase
{
  private readonly IAuthenticationService _authenticationService;

  public AuthenticationController(IAuthenticationService authenticationService) 
  {
    _authenticationService = authenticationService;
  }

  [HttpPost("register")]
  public IActionResult Register(RegisterRequest request) 
  {
    Result<AuthenticationResult> registerResult = _authenticationService.Register(
      request.FirstName, 
      request.LastName, 
      request.Email, 
      request.Password
    );

    if (registerResult.IsSuccess)
    {
      return Ok(MapAuthResult(registerResult.Value));
    }

    var fistError = registerResult.Errors[0];

    if (fistError is DuplicateEmailError)
    {
      return Problem(statusCode: StatusCodes.Status409Conflict, detail: "Email already exists!");
    }
    
    return Problem();
  }

  private AuthenticationResponse MapAuthResult(AuthenticationResult authResult)
  {
    return new AuthenticationResponse(
      authResult.User.Id,
      authResult.User.FirstName,
      authResult.User.LastName,
      authResult.User.Email,
      authResult.Token
    );
  }

  [HttpPost("login")]
  public IActionResult Login(LoginRequest request) 
  {
    var authResult = _authenticationService.Login (
      request.Email,
      request.Password
    );
    var response = new AuthenticationResponse (
      authResult.User.Id,
      authResult.User.FirstName,
      authResult.User.LastName,
      authResult.User.Email,
      authResult.Token
    );
    return Ok(response);
  }
}