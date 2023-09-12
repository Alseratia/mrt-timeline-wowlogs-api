using Newtonsoft.Json;

namespace WarcraftLogsClient;
public record OAuthAccessToken
{
  [JsonProperty("access_token")]
  public string _accessToken { get; init; } = "";
  [JsonProperty("expires_in")]
  public long _expiresIn { get; init; }

  public override string ToString() => _accessToken ?? "";
}