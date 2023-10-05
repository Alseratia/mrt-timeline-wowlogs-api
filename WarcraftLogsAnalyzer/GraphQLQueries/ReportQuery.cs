namespace WarcraftLogsAnalyzer.Query;

public class ReportQuery : BaseQuery<WLReportData>
{
  public ReportQuery(string code, uint fightId, 
                     FilterExpression? bossFilterExpression = null,
                     FilterExpression? playersFilterExpression = null)
  {
    bossFilterExpression?.AndWhere(x => (x.Type == EventType.cast || x.Type == EventType.begincast ||
                           x.Type == EventType.applybuff || x.Type == EventType.removebuff) && x.SourceType == SourceType.Npc);
    if (bossFilterExpression == null) 
      bossFilterExpression = FilterExpression.Where(x => (x.Type == EventType.cast || x.Type == EventType.begincast ||
                           x.Type == EventType.applybuff || x.Type == EventType.removebuff) && x.SourceType == SourceType.Npc);
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
                }}
                playerDetails(fightIDs: {fightId})
                bossEvents:
                events(
                  fightIDs: {fightId},
                  hostilityType: Enemies,
                  limit: 1000000000
                  {$",filterExpression: \"{bossFilterExpression}\""}
                ) {{ data }}
                playersCasts:
                events(
                  fightIDs: {fightId},
                  hostilityType: Friendlies,
                  dataType: Casts
                  limit: 1000000000
                  {(playersFilterExpression == null ? "" : $",filterExpression: \"{playersFilterExpression}\"")}
                ) {{ data }}
              }}
            }}
          }}";
  }
}