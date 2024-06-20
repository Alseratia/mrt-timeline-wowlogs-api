namespace WarcraftLogs.ResponseModels;
public class Player
{
  public string Name { get; set; } = null!;
  public int Id { get; set; }
  public string Type { get; set; } = null!;

  public IEnumerable<PlayerSpec> Specs { get; set; } = null!;
}