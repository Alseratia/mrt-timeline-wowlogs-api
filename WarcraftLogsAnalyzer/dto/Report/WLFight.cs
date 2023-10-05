using Newtonsoft.Json;

namespace WarcraftLogsAnalyzer;

public class WLFight
{
  public int EncounterID { get; set; }
  public string Name { get; set; } = null!;
  [JsonConverter(typeof(TimeSpanMillisecondsConverter))]
  public TimeSpan StartTime { get; set; }
  [JsonConverter(typeof(TimeSpanMillisecondsConverter))]
  public TimeSpan EndTime { get; set; }
  public WLDifficulty? Difficulty { get; set; }
  public int GetFightDuration() => (int)((EndTime - StartTime).TotalSeconds);
}