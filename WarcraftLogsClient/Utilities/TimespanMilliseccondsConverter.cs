using System.Text.Json;
using System.Text.Json.Serialization;

namespace WarcraftLogs.Utilities;

public class TimeSpanMillisecondsConverter : JsonConverter<TimeSpan>
{
  public override TimeSpan Read(ref Utf8JsonReader reader, 
                                Type objectType, 
                                JsonSerializerOptions options)
  {
    if (reader.TokenType == JsonTokenType.Number)
    {
      var milliseconds = reader.GetInt64();
      return TimeSpan.FromMilliseconds(milliseconds);
    }

    throw new JsonException("Expected number or null");
  }

  public override void Write(Utf8JsonWriter writer, 
                             TimeSpan value,
                             JsonSerializerOptions options)
  {
    writer.WriteNumberValue(value.TotalMilliseconds);
  }
}
