namespace WarcraftLogsService.Models;

public class FightEvent
{
  public EventType Type { get; set; }
  public int AbsoluteTimer { get; set; }
  public int Number { get; set; }

  public bool IsActivationEvent(EventType type, int number)
    => Type == type && Number == number;

  public bool IsEndType()
    => Type is EventType.Cast or EventType.RemoveBuff or EventType.RemoveDebuff;
}