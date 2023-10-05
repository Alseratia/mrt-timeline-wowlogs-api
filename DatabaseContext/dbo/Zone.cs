using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimelineDatabaseContext;

[PrimaryKey("Id")]
[Table("Zone")]
public class Zone
{
  public int Id { get; set; }
  public string Name { get; set; } = null!;
  [JsonIgnore]
  public ICollection<Boss>? Boss { get; set; }
}