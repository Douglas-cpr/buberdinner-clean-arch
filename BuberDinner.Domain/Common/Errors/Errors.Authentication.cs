using ErrorOr;

namespace BuberDinner.Domain.Common.Errors;

public static partial class Erros
{
  public static class Authentication
  {
    public static Error InvalidCredentials => Error.Validation(
      code: "User.InvalidCred",
      description: "Invalid credentials."
    );
  }
}