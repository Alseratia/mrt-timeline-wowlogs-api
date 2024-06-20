namespace WebApi;

public class ErrorHandlingMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<ErrorHandlingMiddleware> _logger;

  public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    => (_next, _logger) = (next, logger);

  public async Task Invoke(HttpContext context)
  {
    try
    {
      await _next.Invoke(context);
    }
    catch (Exception ex)
    {
      var message = $"Unhandled exception occured\nPath = {context.Request.Path}\nException = {ex}\n";
      _logger.LogError(message);
      
      context.Response.StatusCode = StatusCodes.Status500InternalServerError;
      context.Response.ContentType = "application/json";
      await context.Response.WriteAsync("An internal server error occurred");
    }
  }
}
