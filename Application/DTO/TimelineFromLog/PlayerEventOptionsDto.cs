using System.Text.Json.Serialization;
using Application.DTO.Enums;
using Json.More;

namespace Application.DTO;

public class PlayerEventOptionsDto
{
  public string? EventAbilityId { get; set; } 
  public int? EventCount { get; set; }
  
  [JsonConverter(typeof(EnumStringConverter<EventTypeForNoteDto>))]
  public EventTypeForNoteDto? EventTypeForNote { get; set; }
}