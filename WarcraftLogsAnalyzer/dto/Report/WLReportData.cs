namespace WarcraftLogsAnalyzer;

public class WLReportData
{
    public WLFight Fight { get; set; } = null!;
    public WLPlayers Players { get; set; } = null!;
    public List<WLEvent> BossEvents { get; set; } = null!;
    public List<WLEvent> PlayersCasts { get; set; } = null!;
}