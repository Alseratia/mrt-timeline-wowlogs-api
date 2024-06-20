using WarcraftLogs.ResponseModels;

namespace WarcraftLogs.Query;

public class EncounterQuery : BaseQuery<EncounterData>
{
  public EncounterQuery(string code, uint fightId)
  {
    Query = $@"
      query {{
        worldData {{
          zones {{
            id
            name
            encounters {{
              id
              name
            }}
          }}
        }}
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
          }}
        }}
      }}";
  }
}
