using System.ComponentModel;

namespace Application.DTO.Enums;

public enum EventTypeForNoteDto
{
  [Description("SCC")] SCC,
  [Description("SCS")] SCS,
  [Description("SAA")] SAA,
  [Description("SAR")] SAR
}