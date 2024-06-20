using WarcraftLogs.ResponseModels;

namespace WarcraftLogs.Query;

public class FightsQuery : BaseQuery<FightsData>
{
  public FightsQuery(string code)
  {
    Query = $@"
      query {{
        reportData {{
          report(code: ""{code}"") {{
            fights {{
              encounterID
              difficulty
            }}
          }}
        }}
      }}";
  }
}
