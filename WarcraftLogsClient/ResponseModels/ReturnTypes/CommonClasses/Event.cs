using System.Text.Json.Serialization;
using WarcraftLogs.Utilities;

namespace WarcraftLogs.ResponseModels;

public class Event
{
  [JsonConverter(typeof(TimeSpanMillisecondsConverter))]
  public TimeSpan Timestamp { get; set; }
  [JsonConverter(typeof(JsonStringEnumConverter))]
  public EventType Type { get; set; }
  public int AbilityGameID { get; set; }
  public int SourceID { get; set; }
}