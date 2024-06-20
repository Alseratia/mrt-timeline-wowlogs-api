namespace WarcraftLogs.ResponseModels;

public class EncounterData
{
  public ReportDataClass ReportData { get; set; } = null!;
  public WorldDataClass WorldData { get; set; } = null!;

  public class ReportDataClass
  {
    /// <summary>
    /// If report does not exist, Report = null
    /// </summary>
    public ReportClass? Report { get; set; }

    public class ReportClass
    {
      /// <summary>
      /// If fightID does not exist, Fights length = 0
      /// </summary>
      public IEnumerable<Fight> Fights { get; set; } = null!;
    }
  }
  public class WorldDataClass
  {
    public IEnumerable<Zone> Zones { get; set; } = null!;
  }
}