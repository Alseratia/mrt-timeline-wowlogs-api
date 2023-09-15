using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
  public int FightDuration { get; set; }
  public int OrderInRaid { get; set; }
  public Difficulty Difficulty { get; set; }
  public FightType FightType { get; set; }

  [JsonIgnore]
  public Zone? Zone { get; set; }
  public ICollection<BossStage> Stages { get; set; } = new List<BossStage>();
  public ICollection<BossAbility> Abilities { get; set; } = new List<BossAbility>();
  public ICollection<BossEvent> Events { get; set; } = new List<BossEvent>();
}