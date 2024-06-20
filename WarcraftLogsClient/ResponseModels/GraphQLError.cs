using System.Text.Json.Serialization;

namespace WarcraftLogs.ResponseModels;

public class GraphQLError
{
  [JsonPropertyName("message")]
  public string Message { get; set; } = null!;
  [JsonPropertyName("extensions")]
  public Dictionary<string, object>? Extensions { get; set; }
  [JsonPropertyName("locations")]
  public IEnumerable<GraphQLLocations> Locations { get; set; } = null!;
  [JsonPropertyName("path")]
  public IEnumerable<string> Path { get; set; } = null!;
}