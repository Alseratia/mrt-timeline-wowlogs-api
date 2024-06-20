using System.Diagnostics;
using WarcraftLogs.ResponseModels;

namespace WarcraftLogs.Query;

public class ReportQuery : BaseQuery<ReportInfo>
{
  public ReportQuery(string code, uint fightId, ICollection<int> filterBossAbilities, ICollection<int> filterPlayersAbilities, int? sourceId = null)
  {
    var bossFilterExpression = FilterExpression.Where(x => (x.Type == EventType.cast || x.Type == EventType.begincast ||
                           x.Type == EventType.applybuff || x.Type == EventType.removebuff) && x.SourceType == SourceType.Npc);

    var bossFilterAbilities = AnyOfExpression(filterBossAbilities);
    if (bossFilterAbilities != null) bossFilterExpression.AndWhere(bossFilterAbilities);

    var playersFilterExpression = AnyOfExpression(filterPlayersAbilities);
    
    
    Query = $@"
          query {{
            reportData {{
              report(code: ""{code}"") {{
                fights(fightIDs: {fightId}) {{
                  encounterID
                  name
                  startTime
                  endTime
                  difficulty
                  gameZone {{ name }}
                }}
                playerDetails(fightIDs: {fightId})
                bossEvents:
                events(
                  fightIDs: {fightId},
                  hostilityType: Enemies,
                  limit: 1000000000,
                  filterExpression: ""{bossFilterExpression}""
                ) {{ data }}
                playersEvents:
                events(
                  fightIDs: {fightId},
                  hostilityType: Friendlies,
                  {(sourceId == null ? "" : $"sourceID: {sourceId},")}
                  dataType: Casts,
                  limit: 1000000000
                  {(playersFilterExpression == null ? "" : $",filterExpression: \"{playersFilterExpression}\"" )}
                ) {{ data }}
              }}
            }}
          }}";
  }

  private FilterExpression? AnyOfExpression(ICollection<int> filterAbilities)
  {
    if (filterAbilities?.Any() != true) return null;

    var first = filterAbilities.FirstOrDefault();
    var playersFilterExpression = FilterExpression.Where(x => x.AbilityId == first);
  
    foreach (var ability in filterAbilities.Skip(1))
    {
      playersFilterExpression.OrWhere(x => x.AbilityId == ability);
    }

    return playersFilterExpression;
  }
}