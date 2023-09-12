using Newtonsoft.Json;
using WarcraftLogsClient;

using WarcraftLogsAnalyzer.Models;

namespace WarcraftLogsAnalyzer;

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
    return (await GetResponseObjectAsync(query))?.Data?.ReportData?.Report?.Fights?[0];
  }

  public async Task<List<WLZone>?> GetAllZonesAndEncountersAsync()
  {
    var query = new AllZonesAndEncountersQuery();
    return (await GetResponseObjectAsync(query))?.Data?.WorldData?.Zones;
  }

  public async Task<WLAbility?> GetAbilityAsync(int Id)
  {
    var query = new AbilityQuery(Id);
    return (await GetResponseObjectAsync(query))?.Data?.GameData?.Ability;
  }
  public async Task<WLPlayers?> GetPlayersAsync(string code, int fightId)
  {
    var query = new PlayersQuery(code, fightId);
    var response = await GetResponseObjectAsync(query);
    NormalizeEventTimers(response);
    return (await GetResponseObjectAsync(query))?.Data?.ReportData?.Report?.PlayerDetails?.Data?.PlayerDetails;
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
    if (responseObject == null) return null;

    var bossEvents = ConcatBossEvents(responseObject);
    if (bossEvents == null) return null;

    NormalizeEventTimers(responseObject);
    if (calcDurationAndNumbers) bossEvents.CalcDurationAndNumber();

    return bossEvents;
  }
  public async Task<List<WLEvent>?> GetPlayersCastsAsync(string code, int fightId)
  {
    var query = new PlayersCastsQuery(code, fightId);
    var responseObject = await GetResponseObjectAsync(query);

    NormalizeEventTimers(responseObject);

    return responseObject?.Data?.ReportData?.Report?.PlayersCasts?.Data;
  }
  public async Task<WLReportData?> GetReportData(string code, int fightId, bool calcDurationAndNumbers)
  {
    var query = new ReportQuery(code, fightId);

    var responseObject = await GetResponseObjectAsync(query);
    FinilizeEvents(responseObject, calcDurationAndNumbers);

    var report = responseObject?.Data?.ReportData?.Report;
    if (report == null || report.Fights?[0] == null ||
      report.BossEvents == null || report.PlayerDetails?.Data?.PlayerDetails == null ||
      report.PlayersCasts?.Data == null) return null;

    return new WLReportData()
    {
      Fight = report.Fights[0],
      BossEvents = report.BossEvents,
      Players = report.PlayerDetails.Data.PlayerDetails,
      PlayersCasts = report.PlayersCasts.Data
    };
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

  private WLResponse? FinilizeEvents(WLResponse? response, bool calcDurationAndNumbers)
  {
    ConcatBossEvents(response);
    NormalizeEventTimers(response);
    if (calcDurationAndNumbers) response?.Data?.ReportData?.Report?.BossEvents?.CalcDurationAndNumber();
    return response;
  }

  private List<WLEvent>? ConcatBossEvents(WLResponse? response)
  {
    var bossCasts = response?.Data?.ReportData?.Report?.BossCasts?.Data;
    var bossBuffs = response?.Data?.ReportData?.Report?.BossBuffs?.Data;
    if (bossCasts == null || bossBuffs == null) return null;
    var bossEvents = bossCasts.Concat(bossBuffs).ToList();
    response!.Data!.ReportData!.Report!.BossEvents = bossEvents;
    return bossEvents;
  }

  private WLResponse? NormalizeEventTimers(WLResponse? response)
  {
    var fight = response?.Data?.ReportData?.Report?.Fights?[0];
    var bossCasts = response?.Data?.ReportData?.Report?.BossCasts?.Data;
    var bossBuffs = response?.Data?.ReportData?.Report?.BossBuffs?.Data;
    var playersCasts = response?.Data?.ReportData?.Report?.PlayersCasts?.Data;

    if (fight == null) return response;

    playersCasts?.ForEach(x => x.Timestamp -= fight.StartTime);
    bossCasts?.ForEach(x => x.Timestamp -= fight.StartTime);
    bossBuffs?.ForEach(x => x.Timestamp -= fight.StartTime);
    return response;
  }

  private WLResponse? CalcBossEventsDuration(WLResponse? response)
  {
    response?.Data?.ReportData?.Report?.BossEvents?.CalcDurationAndNumber();
    return response;
  }
}