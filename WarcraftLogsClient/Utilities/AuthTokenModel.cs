using System.Text.Json.Serialization;

namespace WarcraftLogs.Utilities;

public record OAuthAccessToken
{
  [JsonPropertyName("access_token")]
  public string AccessToken { get; init; } = null!;
  [JsonPropertyName("expires_in")]
  public long ExpiresIn { get; init; }

  public override string ToString() => AccessToken ?? "";
}