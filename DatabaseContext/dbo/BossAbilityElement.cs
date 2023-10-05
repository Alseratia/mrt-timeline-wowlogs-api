using NpgsqlTypes;

namespace TimelineDatabaseContext;

[PgName("BossAbilityElement")]
public enum BossAbilityElement
{
  [PgName("PHYSICAL")] PHYSICAL,
  [PgName("FIRE")] FIRE,
  [PgName("FROST")] FROST,
  [PgName("ARCANE")] ARCANE,
  [PgName("NATURE")] NATURE,
  [PgName("SHADOW")] SHADOW,
  [PgName("HOLY")] HOLY
}
