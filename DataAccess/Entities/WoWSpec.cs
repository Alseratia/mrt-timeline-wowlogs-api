using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DataAccess.Enums;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Entities;

[Table("WoWSpec")]
[Index("Id", Name = "WoWSpec_id_idx")]
[Index("Id", Name = "WoWSpec_id_key", IsUnique = true)]
public class WoWSpec
{
    [Key]
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string InGameClass { get; set; } = null!;

    public string Color { get; set; } = null!;

    public string Icon { get; set; } = null!;
    
    public WoWSpecRole Role { get; set; }
    public string ExpansionId { get; set; } = null!;
    
    [InverseProperty("WowSpec")]
    public virtual ICollection<Ability> Abilities { get; set; } = new List<Ability>();
    
    [ForeignKey("ExpansionId")]
    public virtual Expansion Expansion { get; set; } = null!;
}
