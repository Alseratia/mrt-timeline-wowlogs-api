using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Timeline;
public static class TimelineDbContextExtensions
{
  public static IServiceCollection AddTimelineDatabaseContext(this IServiceCollection services, string? connectionString)
  {
    var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
    dataSourceBuilder.MapEnum<Difficulty>()
                     .MapEnum<FightType>()
                     .MapEnum<EventType>()
                     .MapEnum<EventTypeForNote>()
                     .MapEnum<WoWSpecRole>()
                     .MapEnum<AbilitySet>();
    dataSourceBuilder.MapComposite<Boss>()
                     .MapComposite<BossEvent>()
                     .MapComposite<BossStage>()
                     .MapComposite<WoWSpec>()
                     .MapComposite<Ability>();
    services.AddDbContextFactory<TimelineDbContext>(options => options.UseNpgsql(dataSourceBuilder.Build()));
    return services;
  }
}