using WarcraftLogs.ResponseModels;

namespace WarcraftLogs.Query;

public class AllZonesAndEncountersQuery : BaseQuery<AllZonesAndEncountersData>
{
  public AllZonesAndEncountersQuery()
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
        }}";
  }
}
