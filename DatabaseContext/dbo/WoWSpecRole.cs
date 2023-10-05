using NpgsqlTypes;

namespace TimelineDatabaseContext;

[PgName("WoWSpecRole")]
public enum WoWSpecRole
{
  [PgName("TANK")] TANK,
  [PgName("HEALER")] HEALER,
  [PgName("MELEE")] MELEE,
  [PgName("RANGED")] RANGED
}