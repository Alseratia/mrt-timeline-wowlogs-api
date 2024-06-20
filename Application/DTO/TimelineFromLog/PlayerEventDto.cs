namespace Application.DTO;

public class PlayerEventDto
{
  public int AbsoluteTimer { get; set; }
  public int RelativeTimer { get; set; }
  public string AbilityId { get; set; } = null!;
  public PlayerEventOptionsDto Options { get; set; } = null!;
}