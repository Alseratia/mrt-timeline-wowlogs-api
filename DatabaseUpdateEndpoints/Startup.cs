using Microsoft.EntityFrameworkCore;

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
    services.AddScoped<UpdateDBService>()
            .AddScoped<ToDboTransformer>();

    // register swagger
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
  }

  public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
  {
    // TODO неотловленые исключения в лог middleware
    app.UseSwagger();
    if (env.IsDevelopment())
    {
      app.UseSwaggerUI();
    }

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