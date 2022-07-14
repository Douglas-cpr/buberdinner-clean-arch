using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Services.Authentication.Common;
using BuberDinner.Domain.Entities;
using BuberDinner.Domain.Common.Errors;
using ErrorOr;
using MediatR;
using BuberDinner.Application.Authentication.Queries.Login;

namespace BuberDinner.Application.Authentication.Commands.Register;

public class LoginCommandHandler : 
  IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
  private readonly IJwtTokenGenerator _jwtTokenGenerator;
  private readonly IUserRepository _userRepository;

  public LoginCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
  {
    _jwtTokenGenerator = jwtTokenGenerator;
    _userRepository = userRepository;
  }

  public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
  {
     if (_userRepository.GetUserByEmail(query.Email) is not User user)
    {
      return Erros.Authentication.InvalidCredentials;
    }

    if (user.Password != query.Password)
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
