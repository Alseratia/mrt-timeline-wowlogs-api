using NpgsqlTypes;

namespace DataAccess.Enums;

[PgName("EventTypeForNote")]
public enum EventTypeForNote
{
  [PgName("SCC")] SCC,
  [PgName("SCS")] SCS,
  [PgName("SAA")] SAA,
  [PgName("SAR")] SAR
}