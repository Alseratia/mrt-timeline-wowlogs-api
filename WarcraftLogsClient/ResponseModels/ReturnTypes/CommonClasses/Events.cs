using System.Text.Json.Serialization;
using WarcraftLogs.Utilities;

namespace WarcraftLogs.ResponseModels;

public class Events
{
  public IEnumerable<Event> Data { get; set; } = null!;
}
