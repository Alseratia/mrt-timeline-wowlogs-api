using Newtonsoft.Json;

namespace WarcraftLogs;
public class WLEvent
{
  [JsonConverter(typeof(TimeSpanMillisecondsConverter))]
  public TimeSpan Timestamp { get; set; }
  public WLEventType Type { get; set; }
  public int AbilityGameId { get; set; }
  public int SourceID { get; set; }

  /// <summary>
  /// These fields do not come from warcraft logs 
  /// but are calculated by the extension function.
  /// </summary>
  [JsonIgnore]
  public int Duration { get; set; }
  [JsonIgnore]
  public int CastNumber { get; set; }
}