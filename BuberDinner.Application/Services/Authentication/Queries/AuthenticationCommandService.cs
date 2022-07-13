using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Services.Authentication.Common;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;

namespace BuberDinner.Application.Services.Authentication.Queries;

public class AuthenticationQueryService : IAuthenticationQueryService
{
  private readonly IJwtTokenGenerator _jwtTokenGenerator;
  private readonly IUserRepository _userRepository;

  public AuthenticationQueryService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
  {
    _jwtTokenGenerator = jwtTokenGenerator;
    _userRepository = userRepository;
  }

  public ErrorOr<AuthenticationResult> Login(string email, string password)
  {

    if (_userRepository.GetUserByEmail(email) is not User user)
    {
      return Erros.Authentication.InvalidCredentials;
    }

    if (user.Password != password)
    {
      return Erros.Authentication.InvalidCredentials;
    }

    var token = _jwtTokenGenerator.GenerateToken(user);

    return new AuthenticationResult (
      user,
      token
    );
  }
}