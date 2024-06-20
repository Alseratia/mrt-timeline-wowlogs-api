using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccess.Enums;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Entities;

[Table("Boss")]
[Index("Id", Name = "Boss_id_idx")]
[Index("InGameId", Name = "Boss_inGameId_idx")]
[Index("Name", Name = "Boss_name_idx")]
public partial class Boss
{
    [Key]
    public string Id { get; set; } = null!;

    public int InGameId { get; set; }

    public string Name { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public string Icon { get; set; } = null!;

    public string? Cover { get; set; }

    public int FightDuration { get; set; }

    public int OrderInRaid { get; set; }
    
    public Difficulty Difficulty { get; set; }
    
    public FightType FightType { get; set; }
    

    public string ZoneId { get; set; } = null!;

    [InverseProperty("Boss")]
    public virtual ICollection<BossAbility> BossAbilities { get; set; } = new List<BossAbility>();

    [InverseProperty("Boss")]
    public virtual ICollection<BossStage> BossStages { get; set; } = new List<BossStage>();

    [ForeignKey("ZoneId")]
    [InverseProperty("Bosses")]
    public virtual Zone Zone { get; set; } = null!;
}
