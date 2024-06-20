using NpgsqlTypes;

namespace DataAccess.Enums;

[PgName("BossAbilityElement")]
public enum BossAbilityElement
{
  [PgName("PHYSICAL")] Physical,
  [PgName("FIRE")] Fire,
  [PgName("FROST")] Frost,
  [PgName("ARCANE")] Arcane,
  [PgName("NATURE")] Nature,
  [PgName("SHADOW")] Shadow,
  [PgName("HOLY")] Holy
}