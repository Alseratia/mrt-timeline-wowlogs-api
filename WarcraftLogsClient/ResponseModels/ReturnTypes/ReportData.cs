namespace WarcraftLogs.ResponseModels;

public class ReportInfo
{
  public ReportDataClass ReportData { get; set; } = null!;
  
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
      public RootPlayerDetails PlayerDetails { get; set; } = null!;
      public Events BossEvents { get; set; } = null!;
      public Events PlayersEvents { get; set; } = null!;
    }
  }
}
