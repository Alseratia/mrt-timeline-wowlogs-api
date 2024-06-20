using System.Text.Json.Serialization;
using Application.DTO.Enums;
using Json.More;

namespace Application.DTO;

public class BossAbilityDto
{
  public string Id { get; set; } = null!;

  public int InGameId { get; set; }

  public string Name { get; set; } = null!;

  public string Icon { get; set; } = null!;

  public string? Description { get; set; }

  public string? TooltipImage { get; set; }
    
  [JsonConverter(typeof(EnumStringConverter<BossAbilityElementDto>))]
  public BossAbilityElementDto? Element { get; set; }

  public string? AbilitySet { get; set; }

  public string? ColorForNote { get; set; }

  public string? TextForNote { get; set; }
    
  public bool IsTracked { get; set; }
    
  public bool IsDefaultVisible { get; set; }
  public bool IsOccuringRandomly { get; set; } = false;
  
  public string? BossId { get; set; } = null!;

  [JsonPropertyName("event")]
  public ICollection<BossEventDto> Events { get; set; } = null!;
}