using Amazon.Lambda.AspNetCoreServer;

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