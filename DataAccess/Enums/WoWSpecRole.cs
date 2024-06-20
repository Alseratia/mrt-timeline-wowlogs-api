using NpgsqlTypes;

namespace DataAccess.Enums;

[PgName("WoWSpecRole")]
public enum WoWSpecRole
{
  [PgName("TANK")] Tank,
  [PgName("HEALER")] Healer,
  [PgName("MELEE")] Melee,
  [PgName("RANGED")] Ranged
}