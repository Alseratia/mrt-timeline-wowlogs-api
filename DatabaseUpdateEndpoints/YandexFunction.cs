using Amazon.Lambda.AspNetCoreServer;

using YandexFunction;

namespace WebApplication
{
  public class YandexFunction : YandexGatewayProxyFunction
  {
    protected override void Init(IWebHostBuilder builder)
    {
      builder.ConfigureLogging(builder => builder.AddConsole(options =>
            options.FormatterName = nameof(YandexLoggerFormatter)));
      builder.UseStartup<Startup>();
    }
  }
}