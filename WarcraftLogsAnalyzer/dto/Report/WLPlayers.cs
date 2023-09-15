namespace WarcraftLogs;

public class WLPlayers
{
    public List<WLPlayer> Dps { get; set; } = new List<WLPlayer>();
    public List<WLPlayer> Healers { get; set; } = new List<WLPlayer>();
    public List<WLPlayer> Tanks { get; set; } = new List<WLPlayer>();
}
