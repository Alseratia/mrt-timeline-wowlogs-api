using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Entities;

[Table("Zone")]
[Index("Id", Name = "Zone_id_idx")]
[Index("InGameId", Name = "Zone_inGameId_idx")]
[Index("InGameId", Name = "Zone_inGameId_key", IsUnique = true)]
public partial class Zone
{
    [Key]
    public string Id { get; set; } = null!;

    public int InGameId { get; set; }

    public string Name { get; set; } = null!;

    public bool IsReleased { get; set; }
    
    public string ExpansionId { get; set; } = null!;
    
    [JsonIgnore]
    [InverseProperty("Zone")]
    public virtual ICollection<Boss> Bosses { get; set; } = new List<Boss>();
    
    [ForeignKey("ExpansionId")]
    public virtual Expansion Expansion { get; set; } = null!;
}
