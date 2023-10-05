namespace WarcraftLogsAnalyzer;

public class WLPlayer
{
    public string Name { get; set; } = null!;
    public int Id { get; set; }
    public string Type { get; set; } = null!;

    public List<WLSpec> Specs { get; set; } = new List<WLSpec>();
}
