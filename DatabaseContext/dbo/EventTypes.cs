using NpgsqlTypes;

namespace TimelineDatabaseContext;

[PgName("EventType")]
public enum EventType
{
  begincast, cast, applybuff, removebuff,
  applydebuff, removedebuff, refreshdebuff,
  applybuffstack, removebuffstack, refreshbuff,
  applydebuffstack, removedebuffstack
};

[PgName("EventTypeForNote")]
public enum EventTypeForNote
{
  [PgName("SCC")] SCC,
  [PgName("SCS")] SCS,
  [PgName("SAA")] SAA,
  [PgName("SAR")] SAR
};

public static class NoteTypeExtention
{
  public static EventTypeForNote GetEventTypeForNote(this EventType eventType)
  {
    return eventType switch
    {
      EventType.begincast => EventTypeForNote.SCS,
      EventType.cast => EventTypeForNote.SCC,
      EventType.applybuff => EventTypeForNote.SAA,
      EventType.removebuff => EventTypeForNote.SAR,
      EventType.applydebuff => EventTypeForNote.SAA,
      EventType.removedebuff => EventTypeForNote.SAR,
      _ => throw new ArgumentException($"Not supported event type {eventType}")
    };
  }
}