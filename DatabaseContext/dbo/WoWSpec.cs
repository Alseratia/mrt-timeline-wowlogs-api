using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimelineDatabaseContext;

[PrimaryKey("Id")]
[Table("WoWSpec")]
public class WoWSpec
{
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public string Id { get; set; } = null!;
  public string Name { get; set; } = null!;
  public string InGameClass { get; set; } = null!;
  public string Color { get; set; } = null!;
  public WoWSpecRole Role { get; set; }

  public ICollection<Ability> Abilities { get; set; } = new List<Ability>();
}