namespace WarcraftLogs.ResponseModels;

public class AllZonesAndEncountersData
{
  public WorldDataClass WorldData { get; set; } = null!;
  
  public class WorldDataClass
  {
    public IEnumerable<Zone> Zones { get; set; } = null!;
  }
}