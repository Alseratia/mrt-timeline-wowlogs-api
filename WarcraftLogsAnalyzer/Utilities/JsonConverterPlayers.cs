using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static WarcraftLogsAnalyzer.WLResponse.DataClass.ReportDataClass.ReportClass.PlayerDetailsClass;

namespace WarcraftLogsAnalyzer;
/// <summary>
/// The custom JSON converter is implemented to handle different formats of the "data" field in the JSON object:
/// - When a log is provided without a boss encounter, the "data" field appears as an empty array "[]".
/// - When a log includes a boss encounter, the "data" field takes on the standard object structure defined in the PlayersDetailsData dto.
/// </summary>
public class DataConverterPlayers : JsonConverter<PlayerDetailsData>
{
  public override PlayerDetailsData? ReadJson(JsonReader reader, Type objectType, PlayerDetailsData? existingValue, 
                                             bool hasExistingValue, JsonSerializer serializer)
  {
    var token = JToken.Load(reader);
    var playersToken = token["playerDetails"];

    if (playersToken != null && playersToken.Type == JTokenType.Array && !playersToken.HasValues) return new PlayerDetailsData();

    return token.ToObject<PlayerDetailsData?>();
  }

  public override void WriteJson(JsonWriter writer, PlayerDetailsData? value, JsonSerializer serializer)
  {
    throw new NotImplementedException("Writing is not supported for this converter.");
  }
}
