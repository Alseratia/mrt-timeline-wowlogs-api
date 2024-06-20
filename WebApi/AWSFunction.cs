using Amazon.Lambda.AspNetCoreServer;
using WebApi;

namespace LambdaFunction;

public class LambdaEntryPoint : APIGatewayProxyFunction
{
  protected override void Init(IWebHostBuilder hostBuilder)
  {
    hostBuilder.UseStartup<Startup>();
  }
  
  protected override void Init(IHostBuilder builder)
  {
  }
}