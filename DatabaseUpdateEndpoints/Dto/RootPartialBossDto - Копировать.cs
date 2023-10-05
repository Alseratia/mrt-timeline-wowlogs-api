using TimelineDatabaseContext;

public class RootPartialBossDto
{
  public string ReportCode = null!;
  public string FightNumber = null!;
  public Boss Boss { get; set; } = null!;
  public List<PartialBossStage> PartialStages { get; set; } = null!;
}
