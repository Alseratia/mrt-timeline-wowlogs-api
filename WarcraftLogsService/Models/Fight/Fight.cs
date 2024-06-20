namespace WarcraftLogsService.Models;

public class Fight
{
  public int BossId { get; set; }
  public string BossName { get; set; } = null!;
  public string ZoneName { get; set; } = null!;
  public TimeSpan StartTime { get; set; }
  public TimeSpan EndTime { get; set; }
  public int Duration => (int)((EndTime - StartTime).TotalSeconds);
  public Difficulty Difficulty { get; set; }
  
  public ICollection<FightAbility> BossAbilities { get; set; } = null!;
  public ICollection<FightPlayer> Players { get; set; } = null!;
  
  
  public FightEvent? GetBossEvent(int abilityId, EventType type, int number)
    => GetBossAbility(abilityId)?.GetActivationEvent(type, number);
  
  public FightAbility? GetBossAbility(int abilityId)
    => BossAbilities.FirstOrDefault(x => x.Id == abilityId);
}