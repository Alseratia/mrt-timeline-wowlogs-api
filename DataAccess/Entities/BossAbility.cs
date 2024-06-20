using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DataAccess.Enums;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Entities;

[Table("BossAbility")]
[Index("BossId", Name = "BossAbility_bossId_idx")]
[Index("Id", Name = "BossAbility_id_idx")]
[Index("InGameId", Name = "BossAbility_inGameId_idx")]
public class BossAbility
{
    [Key]
    public string Id { get; set; } = null!;

    public int InGameId { get; set; }

    public string Name { get; set; } = null!;

    public string Icon { get; set; } = null!;

    public string? Description { get; set; }

    public string? TooltipImage { get; set; }
    
    public BossAbilityElement? Element { get; set; }

    public string? AbilitySet { get; set; }

    public string? ColorForNote { get; set; }

    public string? TextForNote { get; set; }
    
    public bool IsTracked { get; set; }
    
    public bool IsDefaultVisible { get; set; }

    public bool IsOccuringRandomly { get; set; } = false;
    
    public string BossId { get; set; } = null!;

    [InverseProperty("Ability")]
    public virtual ICollection<BossEvent> BossEvents { get; set; } = new List<BossEvent>();
    
    [JsonIgnore]
    [ForeignKey("BossId")]
    [InverseProperty("BossAbilities")]
    public virtual Boss Boss { get; set; } = null!;
}
