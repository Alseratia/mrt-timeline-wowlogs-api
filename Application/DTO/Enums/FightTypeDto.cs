using System.ComponentModel;

namespace Application.DTO.Enums;

public enum FightTypeDto
{
  [Description("STAGE_DEPENDANT_TIMERS")] StageDependantTimers,
  [Description("STAGE_INDEPENDANT_TIMERS")] StageIndependantTimers,
  [Description("CLUSTERFUCK")] ClusterFuck
}