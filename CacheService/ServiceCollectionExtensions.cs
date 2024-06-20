using CacheService.CacheProvider;
using CacheService.Repositories;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace CacheService;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddRedisCache(this IServiceCollection services, string redisEndpoint, 
    string redisPort, string redisUser, string redisPassword)
  {
    ConfigurationOptions options = new()
    {
      EndPoints = { { redisEndpoint, int.Parse(redisPort) } },
      User = redisUser,
      Password = redisPassword,
      Ssl = true,
      SslProtocols = System.Security.Authentication.SslProtocols.Tls12,
      AbortOnConnectFail = false
    };
    
    ConnectionMultiplexer muxer = ConnectionMultiplexer.Connect(options);
    services.AddSingleton<CacheService, CacheService>()
      .AddSingleton<ICacheProvider, RedisProvider>()
      .AddSingleton(muxer);

    services.AddScoped<AbilitiesCacheRepository>();
    services.AddScoped<BossCacheRepository>();
    services.AddScoped<SpecsCacheRepository>();
    services.AddScoped<StagesCacheRepository>();
    services.AddScoped<FightCacheRepository>();
    
    return services;
  }
}