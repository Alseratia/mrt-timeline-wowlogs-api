namespace WarcraftLogsService.Models;

public class PlayerSpec
{
  public string Id { get; set; } = null!;

  public string Name { get; set; } = null!;
  public string InGameClass { get; set; } = null!;
  public WoWSpecRole Role { get; set; }
  public string Color { get; set; } = null!;

  public ICollection<PlayerAbility> Abilities = null!;
}