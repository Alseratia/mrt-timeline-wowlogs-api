using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NpgsqlTypes;

namespace TimelineDatabaseContext;

[PrimaryKey("Id")]
[Table("BossStage")]
public class BossStage
{
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public string Id { get; set; } = null!;
  public string BossId { get; set; } = null!;
  public int StageNumber { get; set; }
  public string StageName { get; set; } = null!;
  public EventType? EventType { get; set; }
  public EventTypeForNote? EventTypeForNote { get; set; }
  public int? AbilityId { get; set; }
  public int? EventCount { get; set; }
  public int StartTimer { get; set; }
  public int EndTimer { get; set; }

  [JsonIgnore]
  public Boss? Boss { get; set; }
  [JsonIgnore]
  public List<BossEvent> BossEvents { get; set; } = new();

  public bool IsFirstStage() => AbilityId == null || EventType == null || EventCount == null;
}