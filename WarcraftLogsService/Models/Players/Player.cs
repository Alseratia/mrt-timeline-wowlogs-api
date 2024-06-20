namespace WarcraftLogsService.Models;

public class Player
{
  public string Name { get; set; } = null!;
  
  public PlayerRole Role { get; set; }
  
  public string Class { get; set; } = null!;
  public string Spec { get; set; } = null!;
  public ICollection<PlayerAbility> Abilities { get; set; } = null!;
}