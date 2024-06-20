using System.Text.Json.Serialization;
using Application.DTO.Enums;
using Json.More;

namespace Application.DTO;

public class BossStageDto
{
  public string Id { get; set; } = null!;

  public int StageNumber { get; set; }

  public string StageName { get; set; } = null!;
    
  [JsonConverter(typeof(EnumStringConverter<EventTypeDto>))]
  public EventTypeDto? EventType { get; set; }
    
  [JsonConverter(typeof(EnumStringConverter<EventTypeForNoteDto>))]
  public EventTypeForNoteDto? EventTypeForNote { get; set; }

  public int? AbilityId { get; set; }

  public int? EventCount { get; set; }

  public int StartTimer { get; set; }

  public int EndTimer { get; set; }

  public int? MinTimer { get; set; }

  public int? MaxTimer { get; set; }
    

  public string? BossId { get; set; } = null!;
}