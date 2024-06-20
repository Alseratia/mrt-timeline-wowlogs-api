using NpgsqlTypes;

namespace DataAccess.Enums;

[PgName("FightType")]
public enum FightType
{
  [PgName("STAGE_DEPENDANT_TIMERS")] StageDependantTimers,
  [PgName("STAGE_INDEPENDANT_TIMERS")] StageIndependantTimers,
  [PgName("CLUSTERFUCK")] ClusterFuck
}