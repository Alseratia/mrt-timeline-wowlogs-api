using Source = WarcraftLogs.ResponseModels;
using WarcraftLogsService.Models;

namespace WarcraftLogsService.Mappers;

public static class FightAbilitiesMapper
{
  public static ICollection<FightAbility> Map(this IEnumerable<Source.Event> events, TimeSpan? startFight = null)
  {
    return events.GroupBy(x => x.AbilityGameID).Select(x => x.Map(startFight)).ToList();
  }
  
  public static FightAbility Map(this IGrouping<int, Source.Event> events, TimeSpan? startFight = null)
  {
    var abilityEvents = events.Map(out var abilityDuration, startFight);
    return new FightAbility()
    {
      Id = events.Key,
      Duration = abilityDuration,
      Events = abilityEvents
    };
  }
}