using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;

using WarcraftLogsClient;

public static class WarcraftLogsGraphQLClientExtentions
{
  public static IServiceCollection AddWarcraftLogsGraphQLClient(this IServiceCollection services)
  {
    services.AddHttpClient(
      "WarcraftLogsGraphQLClient", httpClient =>
      {
        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
      });

    services.AddSingleton<WarcraftLogsGraphQLClient>();
    return services;
  }
}