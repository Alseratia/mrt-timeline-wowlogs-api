using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WarcraftLogs.ResponseModels;

public class GraphQLResponseMessage<T> where T : class
{
  public HttpStatusCode StatusCode { get; private set; }
  public string? ReasonPhrase { get; private set; }
  public HttpHeaders Headers { get; private set; }
  public GraphQLContent<T>? Content { get; private set; }
  public bool IsSuccessStatusCode => ((int)StatusCode >= 200) && ((int)StatusCode <= 299);
  public bool IsSuccessGraphQLQuery => IsSuccessStatusCode && Content is not null && (Content.Errors is null || !Content.Errors.Any());
  public bool IsSuccessReturnData => IsSuccessGraphQLQuery && Content?.Data is not null;
  
  public GraphQLResponseMessage(HttpResponseMessage response)
  {
    StatusCode = response.StatusCode;
    ReasonPhrase = response.ReasonPhrase;
    Headers = response.Headers;
    
    var contentString = response.Content.ReadAsStringAsync().Result;
    var options = new JsonSerializerOptions
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
    Stopwatch s = new();
    s.Start();
    Content = JsonSerializer.Deserialize<GraphQLContent<T>>(contentString, options);
    s.Stop();
    Console.WriteLine($"Deserialize: {s.ElapsedMilliseconds}");
  }
}