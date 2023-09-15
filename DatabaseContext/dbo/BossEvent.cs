using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimelineDatabaseContext;

[PrimaryKey("Id")]
[Table("BossEvent")]
public class BossEvent
{
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public string Id { get; set; } = null!;
  public string BossId { get; set; } = null!;
  public EventType EventType { get; set; }
  public EventTypeForNote EventTypeForNote { get; set; }
  public string AbilityId { get; set; } = null!;
  public int AbilityCount { get; set; }
  public int AbsoluteTimer { get; set; }
  public int? RelativeTimer { get; set; }
  public string StageId { get; set; } = null!;

  [JsonIgnore]
  public Boss? Boss { get; set; }
  [JsonIgnore]
  public BossAbility? Ability { get; set; }
  [JsonIgnore]
  public BossStage? Stage { get; set; }
}