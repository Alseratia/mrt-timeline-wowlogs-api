namespace WarcraftLogs.Query;

public class ReportQuery : AbstractQuery<WLReportData>
{
  public ReportQuery(string code, int fightId)
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
                  difficulty
                }}
                playerDetails(fightIDs: {fightId})
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
                playersCasts:
                events(
                  fightIDs: {fightId}
                  hostilityType: Friendlies 
                  dataType: Casts
                  limit: 1000000000
                ) {{ data }}
              }}
            }}
          }}";
  }
}