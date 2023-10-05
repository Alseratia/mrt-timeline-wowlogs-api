using NpgsqlTypes;

namespace TimelineDatabaseContext;

[PgName("Difficulty")]
public enum Difficulty
{
  [PgName("MYTHIC")] MYTHIC = 5,
  [PgName("HEROIC")] HEROIC = 4
}