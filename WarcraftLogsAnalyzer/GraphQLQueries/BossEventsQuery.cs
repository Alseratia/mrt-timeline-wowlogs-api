namespace WarcraftLogsAnalyzer.Query;

public class BossEventsQuery : BaseQuery<List<WLEvent>>
{
  public BossEventsQuery(string code, int fightId, FilterExpression? expression = null)
  {
    expression?.AndWhere(x => (x.Type == EventType.cast || x.Type == EventType.begincast ||
                           x.Type == EventType.applybuff || x.Type == EventType.removebuff) && x.SourceType == SourceType.Npc);
    if (expression == null)
      expression = FilterExpression.Where(x => (x.Type == EventType.cast || x.Type == EventType.begincast ||
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
                }}
                bossEvents:
                events(
                  fightIDs: {fightId},
                  hostilityType: Enemies,
                  limit: 1000000000
                  {$",filterExpression: \"{expression}\""}
                ) {{ data }}
              }}
            }}
          }}";
  }
}