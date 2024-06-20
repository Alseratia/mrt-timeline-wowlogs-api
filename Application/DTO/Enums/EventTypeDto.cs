using System.ComponentModel;

namespace Application.DTO.Enums;

public enum EventTypeDto
{
  [Description("begincast")] BeginCast, 
  [Description("cast")] Cast, 
  [Description("applybuff")] ApplyBuff, 
  [Description("removebuff")] RemoveBuff,
  [Description("applydebuff")] ApplyDebuff, 
  [Description("removedebuff")] RemoveDebuff
}