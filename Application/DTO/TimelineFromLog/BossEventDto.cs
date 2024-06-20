using System.Text.Json.Serialization;
using Application.DTO.Enums;
using Json.More;

namespace Application.DTO;

public class BossEventDto
{
  public string Id { get; set; } = null!;

  public int AbilityCount { get; set; }

  public int AbsoluteTimer { get; set; }

  public int? RelativeTimer { get; set; }

  public int Duration { get; set; }
    
  [JsonConverter(typeof(EnumStringConverter<EventTypeDto>))]
  public EventTypeDto EventType { get; set; }
    
  [JsonConverter(typeof(EnumStringConverter<EventTypeForNoteDto>))]
  public EventTypeForNoteDto EventTypeForNote { get; set; }
    
    
  public string StageId { get; set; } = null!;

  public string AbilityId { get; set; } = null!;
}