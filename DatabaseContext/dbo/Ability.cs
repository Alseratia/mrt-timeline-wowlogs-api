using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace TimelineDatabaseContext;

[PrimaryKey("FieldId")]
[Table("Ability")]
public class Ability
{
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public string FieldId { get; set; } = null!;
  public string Id { get; set; } = null!;
  public string WowSpecId { get; set; } = null!;

  [JsonIgnore]
  public WoWSpec? WoWSpec { get; set; }
}