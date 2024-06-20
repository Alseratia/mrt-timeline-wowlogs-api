
using Shared;
using WarcraftLogsService.Models;
using Source = WarcraftLogs.ResponseModels;


namespace WarcraftLogsService.Mappers;

public static class FightEventsMapper
{
  /// <summary>
  /// Puts down event numbers and returns the duration of the ability
  /// </summary>
  public static ICollection<FightEvent> Map(this IEnumerable<Source.Event> events,
    out int abilityDuration, TimeSpan? startFight = null)
  {
    var abilityEvents = new List<FightEvent>();
    var abilityTypes = events.GroupBy(x => x.GetPairType());
    abilityDuration = 0;

    foreach (var abilityType in abilityTypes)
    {
      if (abilityType.Key == null) continue;

      var startTime = TimeSpan.Zero;
      var number = 1;
      foreach (var abilityEvent in abilityType)
      {
        var convertedEvent = abilityEvent.Map(number, startFight);
        if (convertedEvent == null) continue;
        
        abilityEvents.Add(convertedEvent);
        if (convertedEvent.IsEndType())
        {
          if (startTime != TimeSpan.Zero)
          {
            var duration = (int)(abilityEvent.Timestamp - (startFight ?? TimeSpan.Zero) - startTime).TotalSeconds;
            abilityDuration = Math.Max(duration, abilityDuration);
            startTime = TimeSpan.Zero;
          }
          number++;
        }
        else
        {
          startTime = abilityEvent.Timestamp;
        }
      }
    }
    return abilityEvents;
  }

  public static FightEvent? Map(this Source.Event abilityEvent, int number, TimeSpan? startFight = null)
  {
    var eventType = abilityEvent.Type.Map<EventType>();
    if (eventType == null) return null;
    
    return new FightEvent()
    {
      Type = eventType.Map<EventType>(),
      Number = number,
      AbsoluteTimer = (int)(abilityEvent.Timestamp - startFight ?? TimeSpan.Zero).TotalSeconds
    };
  }
  
  
  public enum EventTypePair { Cast, Buff, DeBuff, NoPair }
  private static EventTypePair? GetPairType(this Source.Event abilityEvent)
  {
    return abilityEvent.Type switch
    {
      WarcraftLogs.EventType.begincast => EventTypePair.Cast,
      WarcraftLogs.EventType.cast => EventTypePair.Cast,
      WarcraftLogs.EventType.applybuff => EventTypePair.Buff,
      WarcraftLogs.EventType.removebuff => EventTypePair.Buff,
      WarcraftLogs.EventType.applydebuff => EventTypePair.DeBuff,
      WarcraftLogs.EventType.removedebuff => EventTypePair.DeBuff,
      _ => null
    };
  }
}