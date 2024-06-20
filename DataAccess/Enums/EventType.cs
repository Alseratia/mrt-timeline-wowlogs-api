using NpgsqlTypes;

namespace DataAccess.Enums;

[PgName("EventType")]
public enum EventType
{
  [PgName("begincast")] BeginCast, 
  [PgName("cast")] Cast, 
  [PgName("applybuff")] ApplyBuff, 
  [PgName("removebuff")] RemoveBuff,
  [PgName("applydebuff")] ApplyDebuff, 
  [PgName("removedebuff")] RemoveDebuff
}