using ErrorOr;

namespace BuberDinner.Domain.Common.Errors;

public static partial class Erros
{
  public static class Authentication
  {
    public static Error InvalidCredentials => Error.Conflict(
      code: "User.InvalidCred",
      description: "Invalid credentials."
    );
  }
}