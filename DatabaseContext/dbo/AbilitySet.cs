using NpgsqlTypes;

namespace TimelineDatabaseContext;

[PgName("AbilitySet")]
public enum AbilitySet
{
  [PgName("RAID_CD")] RAID_CD,
  [PgName("RAID_UTILITY")] RAID_UTILITY
}