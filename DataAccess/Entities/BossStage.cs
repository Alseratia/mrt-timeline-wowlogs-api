using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DataAccess.Enums;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Entities;

[Table("BossStage")]
[Index("BossId", Name = "BossStage_bossId_idx")]
[Index("Id", "StageNumber", Name = "BossStage_id_stageNumber_idx")]
public class BossStage
{
    [Key]
    public string Id { get; set; } = null!;

    public int StageNumber { get; set; }

    public string StageName { get; set; } = null!;
    
    public EventType? EventType { get; set; }
    
    public EventTypeForNote? EventTypeForNote { get; set; }

    public int? AbilityId { get; set; }

    public int? EventCount { get; set; }

    public int StartTimer { get; set; }

    public int EndTimer { get; set; }

    public int? MinTimer { get; set; }

    public int? MaxTimer { get; set; }
    

    public string BossId { get; set; } = null!;
    
    [JsonIgnore]
    [InverseProperty("Stage")]
    public virtual ICollection<BossEvent> BossEvents { get; set; } = new List<BossEvent>();
    
    [JsonIgnore]
    [ForeignKey("BossId")]
    [InverseProperty("BossStages")]
    public virtual Boss Boss { get; set; } = null!;
}
