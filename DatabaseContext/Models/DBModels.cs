using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NpgsqlTypes;

namespace Timeline;

// ------------------------- Boss tables ------------------------- //

[PrimaryKey("id")]
public class Zone
{
  public int id { get; set; }
  public string? name { get; set; }

  public ICollection<Boss> boss { get; set; } = new List<Boss>();
}

[PgName("Difficulty")]
public enum Difficulty
{
  [PgName("MYTHIC")] MYTHIC = 5,
  [PgName("HEROIC")] HEROIC = 4
}

[PgName("FightType")]
public enum FightType
{
  [PgName("STAGE_DEPENDANT_TIMERS")] STAGE_DEPENDANT_TIMERS,
  [PgName("STAGE_INDEPENDANT_TIMERS")] STAGE_INDEPENDANT_TIMERS,
  [PgName("CLUSTERFUCK")] CLUSTERFUCK
}

[PrimaryKey("id")]
public class Boss
{
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public string? id { get; set; }
  public int inGameId { get; set; }
  public int zoneId { get; set; }
  public string name { get; set; } = "";
  // public string? shortName { get; set; }
  // public string icon { get; set; } = "";
  public int fightDuration { get; set; }
  public int orderInRaid { get; set; }
  public Difficulty difficulty { get; set; } = Difficulty.MYTHIC;
  public FightType fightType { get; set; } = FightType.STAGE_DEPENDANT_TIMERS;

  [JsonIgnore]
  public Zone? zone { get; set; }
  public ICollection<BossStage> stages { get; set; } = new List<BossStage>();
  public ICollection<BossAbility> abilities { get; set; } = new List<BossAbility>();
  public ICollection<BossEvent> events { get; set; } = new List<BossEvent>();
}


[PrimaryKey("id")]
public class BossStage
{
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public string? id { get; set; }
  public string? bossId { get; set; }
  public int stageNumber { get; set; }
  public string stageName { get; set; } = "";
  public EventType? eventType { get; set; }
  public EventTypeForNote? eventTypeForNote { get; set; }
  public int? abilityId { get; set; }
  public int? eventCount { get; set; }
  public int startTimer { get; set; }
  public int endTimer { get; set; }

  [JsonIgnore]
  public Boss? boss { get; set; }
  [JsonIgnore]
  public List<BossEvent> bossEvents { get; set; } = new();

  public bool IsFirstStage() => abilityId == null || eventType == null || eventCount == null;
}

// public enum AbilityElement { PHYSICAL, FIRE, FROST, ARCANE, NATURE, SHADOW, HOLY, CHAOS }

[PrimaryKey("id")]
public class BossAbility
{
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public string? id { get; set; }
  public int inGameId { get; set; }
  public string? bossId { get; set; }
  public string name { get; set; } = "";

  public string icon { get; set; } = "";
  public int duration { get; set; }
  public bool isTracked { get; set; }
  [JsonIgnore]
  public Boss? boss { get; set; }
}

[PrimaryKey("id")]
public class BossEvent
{
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public string? id { get; set; }
  public string? bossId { get; set; }
  public EventType eventType { get; set; }
  public EventTypeForNote eventTypeForNote { get; set; }
  public string? abilityId { get; set; }
  public int abilityCount { get; set; }
  public int absoluteTimer { get; set; }
  public int? relativeTimer { get; set; }
  public string stageId { get; set; } = "";

  [JsonIgnore]
  public Boss? boss { get; set; }
  [JsonIgnore]
  public BossAbility? ability { get; set; }
  [JsonIgnore]
  public BossStage? stage { get; set; }
}

// ------------------------- Player tables ------------------------- //
[PgName("WoWSpecRole")]
public enum WoWSpecRole
{
  [PgName("TANK")] TANK,
  [PgName("HEALER")] HEALER,
  [PgName("MELEE")] MELEE,
  [PgName("RANGED")] RANGED
}

[PgName("AbilitySet")]
public enum AbilitySet
{
  [PgName("RAID_CD")] RAID_CD,
  [PgName("RAID_UTILITY")] RAID_UTILITY
}

[PrimaryKey("id")]
public class WoWSpec
{
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public string id { get; set; } = "";
  public string name { get; set; } = "";
  public string inGameClass { get; set; } = "";
  public string color { get; set; } = "";
  public WoWSpecRole role { get; set; }

  public ICollection<Ability> abilities { get; set; } = new List<Ability>();
}

[PrimaryKey("field_id")]
public class Ability
{
  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  public string? field_id { get; set; }
  public string? id { get; set; }
  public string WoWSpecId { get; set; } = "";

  [JsonIgnore]
  public WoWSpec? WoWSpec { get; set; }
}

