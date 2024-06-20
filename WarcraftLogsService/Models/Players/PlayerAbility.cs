namespace WarcraftLogsService.Models;

public class PlayerAbility
{
  public int Id { get; set; }
  public int Duration { get; set; }
  public IEnumerable<PlayerEvent> Events { get; set; } = null!;
}