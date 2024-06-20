namespace WarcraftLogs.ResponseModels;

public class FightsData
{
  public ReportDataClass ReportData { get; set; } = null!;
  public class ReportDataClass
  {
    public ReportClass? Report { get; set; }

    public class ReportClass
    {
      public Fight[] Fights { get; set; } = null!;

      public class Fight
      {
        public int EncounterID { get; set; }
        public Difficulty? Difficulty { get; set; }
      }
    }
  }
}