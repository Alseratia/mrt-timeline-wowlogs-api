using System.Text.Json.Serialization;
using Application.DTO.Enums;
using Json.More;

namespace Application.DTO;

public class PlayerDto
{
  public string SpecId { get; set; } = null!;
  
  [JsonConverter(typeof(EnumStringConverter<WoWSpecRole>))]
  public WoWSpecRole Role { get; set; }
  
  public string Color { get; set; } = null!;
  public string Name { get; set; } = null!;
  public ICollection<PlayerEventDto> Events { get; set; } = null!;
}