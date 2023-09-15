using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace TimelineDatabaseContext;

[PrimaryKey("Id")]
[Table("BossAbility")]
public class BossAbility
{
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public string Id { get; set; } = null!;
  public int InGameId { get; set; }
  public string BossId { get; set; } = null!;
  public string Name { get; set; } = null!;

  public string Icon { get; set; } = null!;
  public int Duration { get; set; }
  public bool IsTracked { get; set; }

  [JsonIgnore]
  public Boss? Boss { get; set; }
}