using System.Text.Json;
using System.Text.Json.Serialization;

namespace WarcraftLogs.Utilities;

/// <summary>
/// The custom JSON converter is implemented to handle different formats of the "data" field in the JSON object:
/// - When a log is provided without a boss encounter, the "data" field appears as an empty array "[]".
/// - When a log includes a boss encounter, the "data" field takes on the standard object structure defined in the PlayersDetailsData dto.
/// </summary>
public class JsonConverterEmptyMassiveToNull<T> : JsonConverter<T?>
{
  public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, 
                              JsonSerializerOptions options)
  {
    if (reader.TokenType == JsonTokenType.StartObject)
    {
      using var doc = JsonDocument.ParseValue(ref reader);
      return JsonSerializer.Deserialize<T>(doc.RootElement.GetRawText(), options);
    }
    if (reader.TokenType == JsonTokenType.StartArray)
    {
      while (reader.Read() && reader.TokenType != JsonTokenType.EndArray) ;
      return default;
    }

    throw new JsonException("Expected StartObject or StartArray token.");
  }

  public override void Write(Utf8JsonWriter writer, T? typeToConvert, JsonSerializerOptions options)
  {
    JsonSerializer.Serialize(writer, typeToConvert, options);
  }
}
