namespace WarcraftLogsAnalyzer.Query;

public class FightQuery : BaseQuery<WLFight>
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
