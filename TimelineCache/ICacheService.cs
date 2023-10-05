using TimelineDatabaseContext;

namespace TimelineCache;
public interface ICacheService
{
  public Boss? GetBossWithIncludes(int bossId, Difficulty difficulty);

  public List<BossAbility>? GetBossAbilities(string bossId);

  public List<BossEvent>? GetBossEvents(string bossId);

  public List<BossStage>? GetBossStages(string bossId);

  public List<WoWSpec>? GetSpecs();

  public List<Ability>? GetAbilities();

  public List<BossStage>? GetBossesStages();
}
