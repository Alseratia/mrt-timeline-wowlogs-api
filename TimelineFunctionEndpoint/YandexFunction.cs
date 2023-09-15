using Amazon.Lambda.AspNetCoreServer;

using Timeline;

namespace WebApplication
{
  public class YandexFunction : YandexGatewayProxyFunction
  {
    protected override void Init(IWebHostBuilder builder)
    {
      builder.UseStartup<Startup>();
    }
  }
}