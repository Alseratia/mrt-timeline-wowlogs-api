using Newtonsoft.Json;

namespace WarcraftLogsAnalyzer;

public class TimeSpanMillisecondsConverter : JsonConverter<TimeSpan>
{
  public override TimeSpan ReadJson(JsonReader reader, Type objectType, 
                                    TimeSpan existingValue, bool hasExistingValue, 
                                    JsonSerializer serializer)
  {
    if (reader.Value == null || reader.Value.ToString() == "") return TimeSpan.Zero;

    long milliseconds = (long)reader.Value;
    TimeSpan timeSpan = TimeSpan.FromMilliseconds(milliseconds);
    return timeSpan;
  }

  public override void WriteJson(JsonWriter writer, TimeSpan value,
                                 JsonSerializer options)
  {
    writer.WriteValue(value.TotalMilliseconds);
  }
}
