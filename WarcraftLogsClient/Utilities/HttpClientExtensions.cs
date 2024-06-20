using System.Text;
using System.Text.Json;

namespace WarcraftLogs.Utilities;

public static class HttpClientGraphQLExtensions
{
  public static async Task<HttpResponseMessage> SendQuery(this HttpClient client, string stringQuery)
  {
    var queryObject = new
    {
      query = stringQuery
    };
    var request = new HttpRequestMessage()
    {
      Method = HttpMethod.Post,
      Content = new StringContent(JsonSerializer.Serialize(queryObject), Encoding.UTF8, "application/json")
        //JsonConvert.SerializeObject(queryObject), Encoding.UTF8, "application/json")
    };
    return await client.SendAsync(request);
  }
}