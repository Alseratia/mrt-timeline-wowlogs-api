namespace WarcraftLogsAnalyzer.Query;

public class FightsQuery : BaseQuery<List<WLFight>>
{
  public FightsQuery(string code)
  {
    Query = $@"
      query {{
        reportData {{
          report(code: ""{code}"") {{
            fights {{
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
