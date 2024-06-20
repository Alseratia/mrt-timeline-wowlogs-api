using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DataAccess.Enums;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Entities;

[Table("BossEvent")]
[Index("Id", Name = "BossEvent_id_idx")]
public class BossEvent
{
    [Key]
    public string Id { get; set; } = null!;

    public int AbilityCount { get; set; }

    public int AbsoluteTimer { get; set; }

    public int? RelativeTimer { get; set; }

    public int Duration { get; set; }
    
    public EventType EventType { get; set; }
    
    public EventTypeForNote EventTypeForNote { get; set; }
    
    
    public string StageId { get; set; } = null!;

    public string AbilityId { get; set; } = null!;
    
    [JsonIgnore]
    [ForeignKey("AbilityId")]
    [InverseProperty("BossEvents")]
    public virtual BossAbility Ability { get; set; } = null!;
    
    [JsonIgnore]
    [ForeignKey("StageId")]
    [InverseProperty("BossEvents")]
    public virtual BossStage Stage { get; set; } = null!;
}
