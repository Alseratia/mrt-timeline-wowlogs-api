namespace WarcraftLogs.Query;

public class BossEventsQuery : AbstractQuery<List<WLEvent>>
{
  public BossEventsQuery(string code, int fightId)
  {
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
                bossCasts:
                events(
                  fightIDs: {fightId}
                  hostilityType: Enemies 
                  dataType: Casts
                  limit: 1000000000
                ) {{ data }}
                bossBuffs:
                events(
                  fightIDs: {fightId}
                  hostilityType: Enemies 
                  dataType: Buffs
                  limit: 1000000000
                ) {{ data }}
              }}
            }}
          }}";
  }
}