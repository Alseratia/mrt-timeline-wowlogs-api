using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Console;

namespace Timeline;

public class Startup
{
  public Startup(IConfiguration configuration)
  {
    Configuration = configuration;
  }
  public IConfiguration Configuration { get; }


  public void ConfigureServices(IServiceCollection services)
  {
    services.AddControllers()
            // I don't like javascript naming convention
            .AddNewtonsoftJson(options =>
                {
                  options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                  options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
                });

    // register other projects DI
    var connectionString = Configuration.GetConnectionString("DbConnection");
    services.AddTimelineDatabaseContext(connectionString)
            .AddWarcraftLogsAnalyzer();

    // register current project DI
    services.AddSingleton<IMemoryCache, MemoryCache>()
            .AddSingleton<ConsoleFormatter, YandexLoggerFormatter>()
            .AddScoped<TimelineService>()
            .AddScoped<ToDtoTransformer>()
            .AddScoped<CacheService>();

    // register swagger
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
  }

  public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
  {
    app.UseSwagger();
    if (env.IsDevelopment()) app.UseSwaggerUI();

    app.UseMiddleware<ErrorHandlingMiddleware>();

    app.UseRouting();

    app.UseEndpoints(endpoints =>
    {
      endpoints.MapControllers();
    });
  }
}