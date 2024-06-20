namespace WarcraftLogs.ResponseModels;

public class AbilityData
{
    public GameDataClass GameData { get; set; } = null!;
    
    public class GameDataClass
    {
        public AbilityClass? Ability { get; set; } = null!;
        
        public class AbilityClass
        {
            public int Id { get; set; }
            public string Name { get; set; } = null!;
            public string Icon { get; set; } = null!;
        }
    }
}