using NpgsqlTypes;

namespace DataAccess.Enums;

[PgName("Difficulty")]
public enum Difficulty
{
  [PgName("MYTHIC")] Mythic = 5,
  [PgName("HEROIC")] Heroic = 4
}