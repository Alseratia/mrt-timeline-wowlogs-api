using Microsoft.Extensions.Logging.Console;
using TimelineCache;

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
            // I don't like javaScript naming convention
            .AddNewtonsoftJson(options =>
                {
                  options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                  options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
                });

    // register other projects DI
    services.AddTimelineDatabaseContext(Configuration)
            .AddWarcraftLogsAnalyzer();

    // register current project DI
    services.AddScoped<TimelineService>()
            .AddSingleton<ConsoleFormatter, YandexLoggerFormatter>()
            .AddScoped<ToDtoTransformer>();

    // Redis cache service
    services.AddRadisTimelineCache(Configuration);

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