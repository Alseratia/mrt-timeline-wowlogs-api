using Newtonsoft.Json;

namespace WarcraftLogsClient;

public record OAuthAccessToken
{
  [JsonProperty("access_token")]
  public string AccessToken { get; init; } = null!;
  [JsonProperty("expires_in")]
  public long ExpiresIn { get; init; }

  public override string ToString() => AccessToken ?? "";
}