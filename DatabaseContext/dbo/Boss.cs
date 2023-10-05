using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TimelineDatabaseContext;

[PrimaryKey("Id")]
[Table("Boss")]
public class Boss
{
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public string Id { get; set; } = null!;
  public int InGameId { get; set; }
  public int ZoneId { get; set; }
  public string Name { get; set; } = null!;
  public string? ShortName { get; set; }
  public string? Icon { get; set; }
  public string? Cover { get; set; }
  public int FightDuration { get; set; }
  public int OrderInRaid { get; set; }
  public Difficulty Difficulty { get; set; }
  public FightType FightType { get; set; }

  public Zone? Zone { get; set; }
  public ICollection<BossStage>? Stages { get; set; }
  public ICollection<BossAbility>? Abilities { get; set; }
  public ICollection<BossEvent>? Events { get; set; }
}