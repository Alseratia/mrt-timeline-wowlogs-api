using System.Text.Json.Serialization;
using WarcraftLogs.Utilities;

namespace WarcraftLogs.ResponseModels;

public class Fight
{
  public int EncounterID { get; set; }
  public string Name { get; set; } = null!;
  [JsonConverter(typeof(TimeSpanMillisecondsConverter))]
  public TimeSpan StartTime { get; set; }
  [JsonConverter(typeof(TimeSpanMillisecondsConverter))]
  public TimeSpan EndTime { get; set; }
  public Difficulty Difficulty { get; set; }
  public GameZone? GameZone { get; set; } = null!;
  
  //public int GetFightDuration() => (int)((EndTime - StartTime).TotalSeconds);
}