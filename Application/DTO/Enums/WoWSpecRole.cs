using System.ComponentModel;

namespace Application.DTO.Enums;

public enum WoWSpecRole
{
  [Description("TANK")] Tank,
  [Description("HEALER")] Healer,
  [Description("MELEE")] Melee,
  [Description("RANGED")] Ranged
}