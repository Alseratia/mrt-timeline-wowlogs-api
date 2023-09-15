using Newtonsoft.Json;

namespace WarcraftLogs;

public record OAuthAccessToken
{
  [JsonProperty("access_token")]
  public string AccessToken { get; init; } = null!;
  [JsonProperty("expires_in")]
  public long ExpiresIn { get; init; }

  public override string ToString() => AccessToken ?? "";
}