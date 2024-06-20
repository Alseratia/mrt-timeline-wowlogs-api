using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DataAccess.Entities;

[Table("Expansion")]
public class Expansion
{
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
}