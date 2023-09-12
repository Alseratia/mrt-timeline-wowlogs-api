using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

using Timeline;
using WarcraftLogsAnalyzer;

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
            .AddNewtonsoftJson(
                options => options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter())
            );

    // register other projects DI
    var connectionString = Configuration.GetConnectionString("DbConnection");
    services.AddTimelineDatabaseContext(connectionString)
            .AddWarcraftLogsAnalyzer();

    // register current project DI
    services.AddSingleton<IMemoryCache, MemoryCache>()
            .AddScoped<TimelineService>()
            .AddScoped<ToTimelineDataTransformer>()
            .AddScoped<CacheService>();

    // register swagger
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
  }

  public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
  {
    app.UseSwagger();
    app.UseSwaggerUI();
    // TODO неотловленые исключения в лог
    // TODO middleware
    // if (app.Environment.IsDevelopment())
    // {
    //   app.UseSwaggerUI();
    // }

    app.UseStatusCodePages();
    app.UseHttpsRedirection();

    app.UseRouting();
    app.UseAuthorization();
    app.UseEndpoints(endpoints =>
    {
      endpoints.MapControllers();
    });
  }
}