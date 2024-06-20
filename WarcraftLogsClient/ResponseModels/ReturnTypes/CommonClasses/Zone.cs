namespace WarcraftLogs.ResponseModels;

public class Zone
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;
  public IEnumerable<Boss>? Encounters { get; set; }
}