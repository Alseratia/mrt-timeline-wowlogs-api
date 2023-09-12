namespace WarcraftLogsAnalyzer.Models;

public class AllZonesAndEncountersQuery : AbstractQuery<List<WLZone>>
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
