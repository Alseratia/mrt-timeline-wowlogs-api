using System.ComponentModel;

namespace Application.DTO.Enums;

public enum BossAbilityElementDto
{
  [Description("PHYSICAL")] Physical,
  [Description("FIRE")] Fire,
  [Description("FROST")] Frost,
  [Description("ARCANE")] Arcane,
  [Description("NATURE")] Nature,
  [Description("SHADOW")] Shadow,
  [Description("HOLY")] Holy
}