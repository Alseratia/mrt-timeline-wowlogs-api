using Application.DTO;
using DataAccess.Entities;
using Mapster;

namespace Application.Mappers;

[Mapper]
public interface IBossMapper
{
  FullBossDto ToFullBossDto(Boss boss);
  Boss ToDbBoss(FullBossDto boss);
}