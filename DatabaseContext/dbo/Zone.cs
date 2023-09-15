using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimelineDatabaseContext;

[PrimaryKey("Id")]
[Table("Zone")]
public class Zone
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;

  public ICollection<Boss> Boss { get; set; } = new List<Boss>();
}