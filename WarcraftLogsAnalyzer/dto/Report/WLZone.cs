namespace WarcraftLogsAnalyzer;

public class WLZone
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<WLBoss> Encounters { get; set; } = new List<WLBoss>();
}
