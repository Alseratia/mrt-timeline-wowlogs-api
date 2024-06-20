using System.Text.Json.Serialization;

namespace Application.DTO;

public class TimelineFromLogDto
{
  public FullBossDto Boss { get; set; } = null!;
  
  [JsonPropertyName("storeState")]
  public Dictionary<string, PlayerDto> Players { get; set; } = null!;
}