using NpgsqlTypes;

namespace TimelineDatabaseContext;

[PgName("FightType")]
public enum FightType
{
  [PgName("STAGE_DEPENDANT_TIMERS")] STAGE_DEPENDANT_TIMERS,
  [PgName("STAGE_INDEPENDANT_TIMERS")] STAGE_INDEPENDANT_TIMERS,
  [PgName("CLUSTERFUCK")] CLUSTERFUCK
}