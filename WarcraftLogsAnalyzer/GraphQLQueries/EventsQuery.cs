namespace WarcraftLogsAnalyzer.Models;

public class EventsQuery : AbstractQuery<List<WLEvent>>
{
  public EventsQuery(string code, int fightId, HostilityType? HostilityType,
                     DataType? EventType, int? sourceId, int? abilityId)
  {
    Query = $@"
      query {{
        reportData {{
          report(code: ""{code}"") {{
            events(
              fightIDs: {fightId}
              {(HostilityType == null ? "" : $",hostilityType: {HostilityType}")} 
              {(EventType == null ? "" : $",dataType: {EventType}")}
              {(sourceId == null ? "" : $",sourceID: {sourceId}")}
              {(abilityId == null ? "" : $",abilityID: {abilityId}")}
            ) {{ data }}
          }}
        }}
      }}";
  }
}
