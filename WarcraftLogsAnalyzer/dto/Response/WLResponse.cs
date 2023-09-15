namespace WarcraftLogs;

public class WLResponse
{
  public DataClass? Data { get; set; }

  public class DataClass
  {
    public WorldDataClass? WorldData { get; set; }
    public GameDataClass? GameData { get; set; }
    public ReportDataClass? ReportData { get; set; }

    public class WorldDataClass
    {
      public List<WLZone>? Zones { get; set; }
    }

    public class GameDataClass
    {
      public WLAbility? Ability { get; set; }
    }

    public class ReportDataClass
    {
      public ReportClass? Report { get; set; }

      public class ReportClass
      {
        public PlayerDetailsClass? PlayerDetails { get; set; }
        public List<WLFight>? Fights { get; set; }
        public PlayersCastsClass? PlayersCasts { get; set; }
        public BossCastsClass? BossCasts { get; set; }
        public BossBuffsClass? BossBuffs { get; set; }
        public List<WLEvent>? BossEvents { get; set; }

        public class PlayerDetailsClass
        {
          public PlayerDetailsData? Data { get; set; }
          public class PlayerDetailsData
          {
            public WLPlayers? PlayerDetails { get; set; }
          }
        }

        public class PlayersCastsClass
        {
          public List<WLEvent>? Data { get; set; }
        }
        public class BossCastsClass
        {
          public List<WLEvent>? Data { get; set; }
        }
        public class BossBuffsClass
        {
          public List<WLEvent>? Data { get; set; }
        }
      }
    }
  }
}