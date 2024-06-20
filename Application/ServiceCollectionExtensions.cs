using Application.DTO;
using Application.UseCases;
using DataAccess.Entities;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ServiceCollectionExtensions
{
  public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
  {
    services.AddScoped<GetFullBoss>();
    services.AddScoped<GetTimelineFromLog>();
    
    TypeAdapterConfig<Boss, FullBossDto>.NewConfig()
      .Map(dest => dest.Abilities, src => src.BossAbilities)
      .Map(dest => dest.Stages, src => src.BossStages)
      .RequireDestinationMemberSource(true);
    
    TypeAdapterConfig<BossAbility, BossAbilityDto>.NewConfig()
      .Map(dest => dest.Events, src => src.BossEvents)
      .RequireDestinationMemberSource(true);
    
    TypeAdapterConfig<FullBossDto, Boss>.NewConfig()
      .Map(dest => dest.BossAbilities, src => src.Abilities)
      .Map(dest => dest.BossStages, src => src.Stages)
      .RequireDestinationMemberSource(true);
    
    TypeAdapterConfig<BossAbilityDto, BossAbility>.NewConfig()
      .Map(dest => dest.BossEvents, src => src.Events)
      .RequireDestinationMemberSource(true);
    
    services.AddMapster();
    
    return services;
  }
}