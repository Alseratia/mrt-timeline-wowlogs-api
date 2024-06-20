using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

using WarcraftLogs.Query;
using WarcraftLogs.ResponseModels;
using WarcraftLogs.Utilities;

namespace WarcraftLogs;

public class WarcraftLogsClient
{
  #region Private fields
  
  private readonly IHttpClientFactory _httpClientFactory;

  private OAuthAccessToken? _accessToken;
  private DateTime _accessTokenExpiration;

  private readonly string _clientId;
  private readonly string _clientSecret;

  private const string OAuthUrl = "https://www.warcraftlogs.com/oauth/token";
  private const string ApiUrl = "https://www.warcraftlogs.com/api/v2/client";
  
  #endregion
  
  #region Constructors
  
  /// <summary>
  /// The "client_id" and "client_secret" fields obtained from WarcraftLogs,
  /// and optionally the "access_token", should be defined in the application configuration.
  /// </summary>
  /// <exception cref="ArgumentNullException">"client_id" or "client_secret" not defined in configuration</exception>
  public WarcraftLogsClient(string clientId, string clientSecret, IHttpClientFactory httpClientFactory, string? currentAccessToken = null)
  {
    _clientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
    _clientSecret = clientSecret ?? throw new ArgumentNullException(nameof(clientSecret));
    _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));

    if (currentAccessToken != null)
      SetAccessToken(new OAuthAccessToken() { AccessToken = currentAccessToken, ExpiresIn = 300000 });
  }

  #endregion

  #region Public methods
  
  /// <param name="query"> The GraphQL query in the graphQL language.
  /// The schema is available at the following link: https://www.warcraftlogs.com/v2-api-docs/warcraft/
  /// </param>
  /// <returns>Deserialized graphql response from WarcraftLogs.</returns>
  public async Task<GraphQLResponseMessage<T>> SendQuery<T>(BaseQuery<T> query) where T : class
  {
    var response = await GetResponseMessage(query.ToString());
    return new GraphQLResponseMessage<T>(response);
  }
  
  /// <param name="query">The GraphQL query in the graphQL language.
  /// The schema is available at the following link: https://www.warcraftlogs.com/v2-api-docs/warcraft/
  /// </param>
  /// <returns>The response from WarcraftLogs.</returns>
  public async Task<GraphQLResponseMessage<object>> SendQuery(string query)
  {
    var response = await GetResponseMessage(query);
    return new GraphQLResponseMessage<object>(response);
  }
  
  #endregion

  #region Private methods

  private async Task<HttpResponseMessage> GetResponseMessage(string query)
  {
    await GetAccessToken();
    Stopwatch s = new();
    var client = _httpClientFactory.CreateClient("WarcraftLogsClient");
    client.BaseAddress = new Uri(ApiUrl);
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken!.ToString());
    s.Start();
    Console.WriteLine($"Start request");
    var result = await client.SendQuery(query);
    s.Stop();
    Console.WriteLine($"Request time: {s.ElapsedMilliseconds}");
    return result;
  }
  
  private async Task GetAccessToken()
  {
    if (_accessToken == null || _accessTokenExpiration < DateTime.Now)
    {
      var client = _httpClientFactory.CreateClient("WarcraftLogsClient");
      var requestBody = new FormUrlEncodedContent(new[]
      {
        new KeyValuePair<string, string>("client_id", _clientId),
        new KeyValuePair<string, string>("client_secret", _clientSecret),
        new KeyValuePair<string, string>("grant_type", "client_credentials"),
      });

      var response = await client.PostAsync(OAuthUrl, requestBody);
      if (!response.IsSuccessStatusCode)
        throw new Exception("WarcraftLogs is not available now");

      var responseContent = await response.Content.ReadAsStringAsync();
      var token = JsonSerializer.Deserialize<OAuthAccessToken>(responseContent, new JsonSerializerOptions()) ?? 
                  throw new Exception("Error deserialize oAuth access token");

      SetAccessToken(token);
    }
  }

  private void SetAccessToken(OAuthAccessToken accessToken)
  {
    _accessToken = accessToken;
    _accessTokenExpiration = DateTime.Now.AddSeconds(_accessToken.ExpiresIn);
  }
  
  #endregion
}
