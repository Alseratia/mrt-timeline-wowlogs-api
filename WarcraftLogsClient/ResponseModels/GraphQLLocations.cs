using System.Text.Json.Serialization;

namespace WarcraftLogs.ResponseModels;

public class GraphQLLocations
{
  [JsonPropertyName("line")]
  public int Line { get; set; }
  [JsonPropertyName("column")]
  public int Column { get; set; }
}