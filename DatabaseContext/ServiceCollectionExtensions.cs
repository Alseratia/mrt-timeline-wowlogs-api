using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

using TimelineDatabaseContext;

public static class DatabaseContextExtensions
{
  public static IServiceCollection AddTimelineDatabaseContext(this IServiceCollection services, IConfiguration configuration)
  {
    var connectionString = configuration["DbConnectionString"] ?? throw new ArgumentNullException("DbConnectionString not set");
    var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

    dataSourceBuilder.MapEnum<Difficulty>()
                     .MapEnum<FightType>()
                     .MapEnum<EventType>()
                     .MapEnum<EventTypeForNote>()
                     .MapEnum<WoWSpecRole>()
                     .MapEnum<AbilitySet>()
                     .MapEnum<BossAbilityElement>();
    dataSourceBuilder.MapComposite<Boss>()
                     .MapComposite<BossEvent>()
                     .MapComposite<BossAbility>()
                     .MapComposite<BossStage>()
                     .MapComposite<WoWSpec>()
                     .MapComposite<Ability>();

    services.AddDbContextFactory<DatabaseContext>(options => options.UseNpgsql(dataSourceBuilder.Build())
                                                                    .UseCamelCaseNamingConvention());
    return services;
  }
}