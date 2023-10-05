namespace WarcraftLogsAnalyzer.Query;

public class AllZonesAndEncountersQuery : BaseQuery<List<WLZone>>
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
