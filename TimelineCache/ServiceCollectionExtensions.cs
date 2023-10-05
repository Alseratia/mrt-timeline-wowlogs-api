using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace TimelineCache;

public static class WarcraftLogsGraphQLClientExtentions
{
  public static IServiceCollection AddRadisTimelineCache(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddTimelineDatabaseContext(configuration);
    ConfigurationOptions options = new()
    {
        EndPoints = { { configuration["RedisEndpoint"]!, Int32.Parse(configuration["RedisPort"]!) } },
        User = configuration["RedisUser"],
        Password = configuration["RedisPassword"],
        Ssl = true,
        SslProtocols = System.Security.Authentication.SslProtocols.Tls12
    };
    ConnectionMultiplexer muxer = ConnectionMultiplexer.Connect(options);
    services.AddSingleton<ICacheService, RedisCacheService>()
            .AddSingleton(muxer);

    return services;
  }

  public static IServiceCollection AddMemotyTimelineCache(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddTimelineDatabaseContext(configuration);
    services.AddMemoryCache();
    return services;
  }
}