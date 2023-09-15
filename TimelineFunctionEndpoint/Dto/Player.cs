using Newtonsoft.Json;

namespace Timeline;

public class Player
{
  [JsonIgnore]
  public int Id { get; set; }
  public string SpecId { get; set; } = null!;
  public string Color { get; set; } = null!;
  public string Name { get; set; } = null!;
  public ICollection<TimelineEvent> Events { get; set; } = new List<TimelineEvent>();
}