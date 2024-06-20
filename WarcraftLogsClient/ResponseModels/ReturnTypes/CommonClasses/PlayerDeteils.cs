using System.Text.Json.Serialization;
using WarcraftLogs.Utilities;

namespace WarcraftLogs.ResponseModels;

public class RootPlayerDetails
{
  public PlayerDetailsData Data { get; set; } = null!;
}

public class PlayerDetailsData
{
  /// <summary>
  /// If fightID does not exist, Players = null
  /// </summary>
  [JsonConverter(typeof(JsonConverterEmptyMassiveToNull<Players>))]
  public Players? PlayerDetails { get; set; }
}