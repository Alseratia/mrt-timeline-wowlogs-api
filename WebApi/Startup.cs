using System.Text.Json;
using System.Text.Json.Serialization;
using Application;
using CacheService;
using DataAccess;
using WarcraftLogsService;

namespace WebApi;

public class Startup
{
  public Startup(IConfiguration configuration) => Configuration = configuration;
  
  private IConfiguration Configuration { get; }
  
  public void ConfigureServices(IServiceCollection services)
  {
    services.AddResponseCompression(options =>
    {
      options.EnableForHttps = true;
    });
    
    services.AddControllers()
      .AddJsonOptions(options =>
      {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
      });

    services.AddAeonsDatabaseContext(Configuration["DbConnectionString"]!);
    services.AddWarcraftLogsService(Configuration["client_id"]!,Configuration["client_secret"]!,
      Configuration["access_token"]!);
    services.AddRedisCache(Configuration["RedisEndpoint"]!,Configuration["RedisPort"]!,
    Configuration["RedisUser"]!,Configuration["RedisPassword"]!);

    services.AddApplicationLayer();
    
    // register swagger
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
  }

  public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
  {
    app.UseSwagger();
    if (env.IsDevelopment()) app.UseSwaggerUI();

    app.UseMiddleware<ErrorHandlingMiddleware>();
    
    app.UseResponseCompression();
    
    app.UseRouting();

    app.UseEndpoints(endpoints =>
    {
      endpoints.MapControllers();
    });
  }
}