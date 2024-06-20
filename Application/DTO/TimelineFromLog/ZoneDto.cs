namespace Application.DTO;

public class ZoneDto
{
  public string Id { get; set; } = null!;

  public int InGameId { get; set; }

  public string Name { get; set; } = null!;

  public bool IsReleased { get; set; }

  public ExpansionDto Expansion { get; set; } = null!;
}