namespace Timeline;

public class ErrorHandlingMiddleware
{
  private readonly RequestDelegate _next;
  private readonly ILogger<ErrorHandlingMiddleware> _logger;

  public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
  {
    _next = next;
    _logger = logger;
  }

  public async Task Invoke(HttpContext context)
  {
    try
    {
      throw new Exception("Hehe");
      await _next.Invoke(context);
    }
    catch (Exception ex)
    {
      _logger.LogError($"Unhandled exception occured\nPath = {context.Request.Path}\nException = {ex}\n");

      context.Response.StatusCode = StatusCodes.Status500InternalServerError;
      context.Response.ContentType = "text/plain";
      await context.Response.WriteAsync("An internal server error occurred");
    }
  }
}
