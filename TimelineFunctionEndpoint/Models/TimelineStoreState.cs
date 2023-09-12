using Newtonsoft.Json;
using Timeline;

/// <summary>
/// The model of the client part of the application
/// </summary>

public class RootTimelineObject
{
  public Dictionary<string, Player> timelineStoreState { get; set; } = new();
  public Boss boss { get; set; } = new();
}

public class Player
{
  [JsonIgnore]
  public int id { get; set; }
  public string specId { get; set; } = "";
  public string color { get; set; } = "";
  public string name { get; set; } = "";
  public ICollection<TimelineEvent> events { get; set; } = new List<TimelineEvent>();
}

public class TimelineEvent
{
  public int absoluteTimer { get; set; }
  public int relativeTimer { get; set; }
  public string spellId { get; set; } = "";
  public TimerOptions options { get; set; } = new();
}

public class TimerOptions
{
  public string? eventSpellId { get; set; }
  public int? castCount { get; set; }
  [JsonProperty("event")]
  public EventTypeForNote? condEvent { get; set; } = null;
}