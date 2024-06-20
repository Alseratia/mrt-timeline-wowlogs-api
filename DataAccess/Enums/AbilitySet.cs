using NpgsqlTypes;

namespace DataAccess.Enums;

[PgName("AbilitySet")]
public enum AbilitySet
{
  [PgName("RAID_CD")] RaidCd,
  [PgName("RAID_UTILITY")] RaidUtility,
  [PgName("PERSONAL")] Personal,
  [PgName("EXTERNAL")] External
}