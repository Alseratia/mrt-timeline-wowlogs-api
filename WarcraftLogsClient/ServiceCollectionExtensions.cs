using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WarcraftLogs;

public static class WarcraftLogsClientExtensions
{
  public static IServiceCollection AddWarcraftLogsClient(this IServiceCollection services, string clientId, string clientSecret, string? accessToken = null)
  {
    services.AddHttpClient(
      "WarcraftLogsClient", httpClient =>
      {
        httpClient.DefaultRequestVersion = HttpVersion.Version20;
        httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("br"));
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      }).ConfigurePrimaryHttpMessageHandler(x => new HttpClientHandler()
      {
        AutomaticDecompression = DecompressionMethods.Brotli
      }); 

    services.AddSingleton<WarcraftLogsClient>(provider =>
    {
      var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
      return new WarcraftLogsClient(clientId, clientSecret, httpClientFactory, accessToken);
    });
    
    return services;
  }
}