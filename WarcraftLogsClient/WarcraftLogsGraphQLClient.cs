using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace WarcraftLogsClient;

/// <summary>
/// Represents a client for accessing the World of Warcraft combat logs through the WarcraftLogs GraphQL API.
/// In the application that uses this client, you need to add the client_id and client_secret fields to the application configuration. 
/// These values are access tokens for the WarcraftLogs API.
/// </summary>
public class WarcraftLogsGraphQLClient
{
  private readonly IHttpClientFactory _httpClientFactory;
  private OAuthAccessToken? _accessToken;
  private DateTime _accessTokenExpiration;
  private readonly string _clientId;
  private readonly string _clientSecret;
  private readonly string _oAuthUrl = "https://www.warcraftlogs.com/oauth/token";

  private readonly string _apiUrl = "https://www.warcraftlogs.com/api/v2/client";

  public WarcraftLogsGraphQLClient(IConfiguration configuration, IHttpClientFactory clientFactory)
      : this(configuration["client_id"]!, configuration["client_secret"]!, clientFactory)
  { }

  public WarcraftLogsGraphQLClient(string clientId, string clientSecret, IHttpClientFactory httpClientFactory)
  {
    _clientId = clientId ?? throw new ArgumentNullException(nameof(clientId));
    _clientSecret = clientSecret ?? throw new ArgumentNullException(nameof(clientSecret));
    _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
  }

  public async Task<HttpResponseMessage> GetAsync(string graphQLQuery)
  {
    return await GetResponce(graphQLQuery);
  }

  private async Task<HttpResponseMessage> GetResponce(string graphQLQuery)
  {
    await GetAccessToken();
    HttpClient client = _httpClientFactory.CreateClient("WarcraftLogsGraphQLClient");
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessToken?.ToString());
    return await client.GetAsync($"{_apiUrl}?query={Uri.EscapeDataString(graphQLQuery)}");
  }

  private async Task GetAccessToken()
  {
    if (_accessToken == null || _accessTokenExpiration < DateTime.Now)
    {
      HttpClient client = _httpClientFactory.CreateClient("WarcraftLogsGraphQLClient");
      var requestBody = new FormUrlEncodedContent(new[]
      {
        new KeyValuePair<string, string>("client_id", _clientId),
        new KeyValuePair<string, string>("client_secret", _clientSecret),
        new KeyValuePair<string, string>("grant_type", "client_credentials"),
      });

      var response = await client.PostAsync(_oAuthUrl, requestBody);
      if (!response.IsSuccessStatusCode)
        throw new Exception("WarcraftLogs is not available now");

      var responseContent = await response.Content.ReadAsStringAsync();
      _accessToken = JsonConvert.DeserializeObject<OAuthAccessToken>(responseContent) ??
        throw new Exception("WarcraftLogs is not available now");

      _accessTokenExpiration = DateTime.Now.AddSeconds(_accessToken._expiresIn);
    }
  }

}
