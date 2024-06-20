namespace WarcraftLogs.ResponseModels;

public class Players
{
  public IEnumerable<Player>? Dps { get; set; }
  public IEnumerable<Player>? Healers { get; set; }
  public IEnumerable<Player>? Tanks { get; set; }
}