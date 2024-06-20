using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;

namespace DataAccess.Entities;

[Table("Ability")]
[Index("Id", Name = "Ability_fieldId_idx")]
public class Ability
{
    [Key]
    public string FieldId { get; set; } = null!;
    
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Icon { get; set; } = null!;

    public string? Description { get; set; }

    public string Cooldown { get; set; } = null!;

    public string Duration { get; set; } = null!;

    public string[] ExtraIds { get; set; } = null!;

    public string WowSpecId { get; set; } = null!;

    [JsonIgnore]
    [ForeignKey("WowSpecId")]
    [InverseProperty("Abilities")]
    public virtual WoWSpec? WowSpec { get; set; }
}
