using Application.DTO;
using DataAccess.Entities;
using Mapster;

namespace Application.MapperConfig;

public class RegisterMapper : IRegister
{
  public void Register(TypeAdapterConfig config)
  {
    config.NewConfig<FullBossDto, Boss>()
      .Map(dest => dest.BossAbilities, src => src.Abilities)
      .Map(dest => dest.BossStages, src => src.Stages)
      .RequireDestinationMemberSource(true);
    
    config.NewConfig<Boss, FullBossDto>()
      .Map(dest => dest.Abilities, src => src.BossAbilities)
      .Map(dest => dest.Stages, src => src.BossStages)
      .RequireDestinationMemberSource(true);
  }
}