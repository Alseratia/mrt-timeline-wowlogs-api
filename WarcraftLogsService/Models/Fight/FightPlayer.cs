namespace WarcraftLogsService.Models;

public class FightPlayer
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;
  public PlayerRole Role { get; set; }
  public string Class { get; set; } = null!;
  public string Spec { get; set; } = null!;

  public IEnumerable<FightAbility> Abilities { get; set; } = null!;
}