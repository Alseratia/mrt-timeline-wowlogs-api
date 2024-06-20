using DataAccess.Context;
using DataAccess.Entities;
using DataAccess.Enums;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace DataAccess;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddAeonsDatabaseContext(this IServiceCollection services, string connectionString)
  {
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
      .MapComposite<Ability>()
      .MapComposite<Zone>();
    
    services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(dataSourceBuilder.Build())
                                                                         .UseCamelCaseNamingConvention());

    services.AddScoped<BossRepository>()
      .AddScoped<AbilitiesRepository>()
      .AddScoped<StagesRepository>()
      .AddScoped<SpecsRepository>();
    
    return services;
  }
}