using Newtonsoft.Json;
using WarcraftLogs.Query;

namespace WarcraftLogs;

/// <summary>
/// The class is designed to send pre-written queries via the warcraft logs client and convert the response into a convenient form.
/// </summary>
public class WarcraftlogsAnalyzer
{
  private readonly WarcraftLogsGraphQLClient _warcraftLogsClient;

  public WarcraftlogsAnalyzer(WarcraftLogsGraphQLClient WarcraftLogsGraphQLClient)
    => _warcraftLogsClient = WarcraftLogsGraphQLClient;

  public async Task<HttpResponseMessage> SendQueryAsync(string query)
    => await _warcraftLogsClient.GetAsync(query);

  public async Task<WLFight?> GetFightAsync(string code, int fightId)
  {
    var query = new FightQuery(code, fightId);
    var responce = await GetResponseObjectAsync(query);
    return responce.GetFight();
  }

  public async Task<List<WLZone>?> GetAllZonesAndEncountersAsync()
  {
    var query = new AllZonesAndEncountersQuery();
    var responce = await GetResponseObjectAsync(query);
    return responce.GetZones();
  }

  public async Task<WLAbility?> GetAbilityAsync(int Id)
  {
    var query = new AbilityQuery(Id);
    var responce = await GetResponseObjectAsync(query);
    return responce.GetAbility();
  }

  public async Task<WLPlayers?> GetPlayersAsync(string code, int fightId)
  {
    var query = new PlayersQuery(code, fightId);
    var response = await GetResponseObjectAsync(query);

    return response.GetPlayers();
  }

  public async Task<WLZone?> FindBossZoneAsync(int bossId)
  {
    var zones = await GetAllZonesAndEncountersAsync();
    if (zones == null) return null;
    return zones.FirstOrDefault(x => x.Encounters.Any(e => e.Id == bossId));
  }

  public async Task<List<WLEvent>?> GetBossEventsAsync(string code, int fightId, bool calcDurationAndNumbers)
  {
    var query = new BossEventsQuery(code, fightId);
    var responseObject = await GetResponseObjectAsync(query);

    responseObject.FinilizeEvents(true);

    return responseObject.GetBossEvents();
  }

  public async Task<List<WLEvent>?> GetPlayersCastsAsync(string code, int fightId)
  {
    var query = new PlayersCastsQuery(code, fightId);
    var response = await GetResponseObjectAsync(query);

    response.FinilizeEvents(false);

    return response.GetPlayersCasts();
  }

  public async Task<WLReportData?> GetReportData(string code, int fightId, bool calcDurationAndNumbers)
  {
    var query = new ReportQuery(code, fightId);

    var response = await GetResponseObjectAsync(query);
    response.FinilizeEvents(calcDurationAndNumbers);

    return response.GetReportData();
  }

  private async Task<string> GetResponseAsync(string query)
  {
    var response = await _warcraftLogsClient.GetAsync(query);
    if (response.IsSuccessStatusCode)
    {
      return await response.Content.ReadAsStringAsync();
    }
    throw new Exception("WarcraftLogs BadRequest");
  }

  private async Task<WLResponse?> GetResponseObjectAsync(string query)
  {
    var response = await GetResponseAsync(query);
    var result = JsonConvert.DeserializeObject<WLResponse>(response);
    return result;
  }
}