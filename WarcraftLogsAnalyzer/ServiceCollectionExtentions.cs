using Microsoft.Extensions.DependencyInjection;

using WarcraftLogsAnalyzer;

public static class WarcraftLogsAnalyzerExtensions
{
  public static IServiceCollection AddWarcraftLogsAnalyzer(this IServiceCollection services)
  {
    services.AddWarcraftLogsGraphQLClient();
    services.AddScoped<WarcraftlogsAnalyzer>();

    return services;
  }
}