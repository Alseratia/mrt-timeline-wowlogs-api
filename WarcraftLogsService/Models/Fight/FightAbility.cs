namespace WarcraftLogsService.Models;

public class FightAbility
{
  public int Id { get; set; }
  public int Duration { get; set; }
  public ICollection<FightEvent> Events { get; set; } = null!;
  
  public FightEvent? GetActivationEvent(EventType type, int number)
  => Events.FirstOrDefault(x => x.IsActivationEvent(type, number));
}