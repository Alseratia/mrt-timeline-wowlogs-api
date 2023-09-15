namespace WarcraftLogs.Query;

public class FightQuery : AbstractQuery<WLFight>
{
  public FightQuery(string code, int fightId)
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
          }}
        }}
      }}";
  }
}
