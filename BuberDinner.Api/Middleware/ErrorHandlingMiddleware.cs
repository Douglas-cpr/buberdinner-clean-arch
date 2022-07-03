using System.Net;
using Newtonsoft.Json;

namespace BuberDinner.Api.Middleware;

public class ErrorHandlingMiddleware
{
  private readonly RequestDelegate _next;

  public ErrorHandlingMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  public async Task Invoke(HttpContext context) 
  {
    try
    {
      await _next(context);
    }
    catch (Exception ex)
    {
      await HandleExceptionAsync(context, ex);
    }
  }

  private static Task HandleExceptionAsync(HttpContext context, Exception exception)
  {
    var code = HttpStatusCode.InternalServerError;
    var result = JsonConvert.SerializeObject(new { error = "An error occurrred while processing your request" });
    context.Response.ContentType = "application/json";
    context.Response.StatusCode = (int)code;
    return context.Response.WriteAsync(result);
  }
}