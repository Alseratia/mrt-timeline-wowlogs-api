using TimelineCache;

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
            .AddNewtonsoftJson(options =>
            {
              options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
              options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            });

    // register other projects DI
    services.AddTimelineDatabaseContext(Configuration)
            .AddRadisTimelineCache(Configuration)
            .AddWarcraftLogsAnalyzer();

    // register current project DI
    services.AddScoped<UpdateDBService>()
            .AddScoped<ToDboTransformer>();

    // swagger
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