namespace WarcraftLogsService.Models;

public static class EventTypeExtensions
{
  public static EventTypeForNote ForNote(this EventType type)
  {
    return type switch
    {
      EventType.BeginCast => EventTypeForNote.SCS,
      EventType.ApplyBuff => EventTypeForNote.SAA,
      EventType.ApplyDebuff => EventTypeForNote.SAA,
      EventType.Cast => EventTypeForNote.SCC,
      EventType.RemoveBuff => EventTypeForNote.SAR,
      EventType.RemoveDebuff => EventTypeForNote.SAR,
      _ => throw new Exception($"Нет типа для заметки")
    };
  }
  
  public static bool IsBeginEvent(this EventType type)
  {
    return type switch
    {
      EventType.BeginCast => true,
      EventType.ApplyBuff => true,
      EventType.ApplyDebuff => true,
      _ => false
    };
  }
  
  public static bool IsEndEvent(this EventType type)
  {
    return !type.IsBeginEvent();
  }

  public static EventType GetPair(this EventType type)
  {
    return type switch
    {
      EventType.BeginCast => EventType.Cast,
      EventType.ApplyBuff => EventType.RemoveBuff,
      EventType.ApplyDebuff => EventType.RemoveDebuff,
      EventType.Cast => EventType.BeginCast,
      EventType.RemoveBuff => EventType.ApplyBuff,
      EventType.RemoveDebuff => EventType.ApplyDebuff,
      _ => throw new Exception($"Нет реализации получения пары для типа ивента {type}")
    };
  }

  public static bool IsPair(this EventType firstType, EventType secondType)
  {
    return (firstType, secondType) switch
    {
      (EventType.BeginCast, EventType.Cast) => true,
      (EventType.ApplyBuff, EventType.RemoveBuff) => true,
      (EventType.ApplyDebuff, EventType.RemoveDebuff) => true,
      (EventType.Cast, EventType.BeginCast) => true,
      (EventType.RemoveBuff, EventType.ApplyBuff) => true,
      (EventType.RemoveDebuff, EventType.ApplyDebuff) => true,
      _ => false
    };
  }
}