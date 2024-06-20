using System.Text.Json.Serialization;
using Application.DTO.Enums;
using Json.More;

namespace Application.DTO;

public class FullBossDto
{
  public string? Id { get; set; } = null!;

  public int? InGameId { get; set; }

  public string Name { get; set; } = null!;

  public string ShortName { get; set; } = null!;

  public string Icon { get; set; } = null!;

  public string? Cover { get; set; }

  public int FightDuration { get; set; }

  public int? OrderInRaid { get; set; }
    
  [JsonConverter(typeof(EnumStringConverter<DifficultyDto>))]
  public DifficultyDto Difficulty { get; set; }
    
  [JsonConverter(typeof(EnumStringConverter<FightTypeDto>))]
  public FightTypeDto FightType { get; set; }
  
  public string? ZoneId { get; set; }
  
  public ICollection<BossAbilityDto> Abilities { get; set; } = null!;

  public ICollection<BossStageDto> Stages { get; set; } = null!;
  
  public ZoneDto? Zone { get; set; } = null!;
}