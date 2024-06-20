using System.Text.Json.Serialization;

namespace WarcraftLogs.ResponseModels;

public class GraphQLContent<TReturnType>
{
  [JsonPropertyName("errors")]
  public IEnumerable<GraphQLError>? Errors { get; set; }
  [JsonPropertyName("data")]
  public TReturnType? Data { get; set; }
  [JsonPropertyName("extensions")]
  public Dictionary<string, object>? Extensions { get; set; } = null!;
}