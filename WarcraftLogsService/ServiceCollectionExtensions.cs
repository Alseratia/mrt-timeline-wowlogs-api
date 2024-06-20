using Microsoft.Extensions.DependencyInjection;
using WarcraftLogs;
using WarcraftLogsService.Repositories;

namespace WarcraftLogsService;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddWarcraftLogsService(this IServiceCollection services,
    string clientId, string clientSecret, string accessToken)
  {
    services.AddWarcraftLogsClient(clientId, clientSecret, accessToken);
    services.AddScoped<FightRepository>();
    return services;
  }
}