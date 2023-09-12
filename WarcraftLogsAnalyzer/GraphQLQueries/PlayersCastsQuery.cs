namespace WarcraftLogsAnalyzer.Models;
public class PlayersCastsQuery : AbstractQuery<List<WLEvent>>
{
  public PlayersCastsQuery(string code, int fightId)
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