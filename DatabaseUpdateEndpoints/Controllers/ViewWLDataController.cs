using Microsoft.AspNetCore.Mvc;
using WarcraftLogsAnalyzer;

/// <summary>
/// Controller for viewing data from Warcraftlogs
/// </summary>
public class ViewWLDataController : Controller
{
  private readonly WarcraftlogsAnalyzer _logAnalyzer;
  public ViewWLDataController(WarcraftlogsAnalyzer logAnalyzer)
  {
    _logAnalyzer = logAnalyzer;
  }

  /// <summary>
  /// Get all encounters from warcraftlogs
  /// </summary>
  [HttpGet("all-encounters", Name = "GetAllEncounters")]
  public async Task<IActionResult> GetAllEncounters()
  {
    return Ok(await _logAnalyzer.GetAllZonesAndEncountersAsync());
  }

  /// <summary>
  /// Get fight data from warcraftlogs
  /// </summary>
  [HttpGet("fight-data/{code}/{fightId}", Name = "GetFightData")]
  public async Task<IActionResult> GetFightData(string code, int fightId)
  {
    return Ok(await _logAnalyzer.GetFightAsync(code, fightId));
  }


  /// <summary>
  /// Get fight data from warcraftlogs
  /// </summary>
  [HttpGet("boss-events/{code}/{fightId}", Name = "GetBossEvents")]
  public async Task<IActionResult> GetBossEvents(string code, int fightId)
  {
    return Ok(await _logAnalyzer.GetBossEventsAsync(code, fightId, true));
  }

  /// <summary>
  /// Get ability data from warcraftlogs
  /// </summary>
  [HttpGet("ability-data/{id}", Name = "GetAbility")]
  public async Task<IActionResult> GetAbility(int id)
  {
    return Ok(await _logAnalyzer.GetAbilityAsync(id));
  }

  /// <summary>
  /// Get players from log
  /// </summary>
  [HttpGet("players/{code}/{fightId}", Name = "GetPlayers")]
  public async Task<IActionResult> GetPlayers(string code, int fightId)
  {
    return Ok(await _logAnalyzer.GetPlayersAsync(code, fightId));
  }

  /// <summary>
  /// Get players casts from log
  /// </summary>
  [HttpGet("players-casts/{code}/{fightId}", Name = "GetPlayersCasts")]
  public async Task<IActionResult> GetPlayersCasts(string code, int fightId)
  {
    return Ok(await _logAnalyzer.GetPlayersCastsAsync(code, fightId));
  }


  /// <summary>
  /// Send query to warcraftlogs and return response
  /// </summary>
  [HttpGet("query={query}", Name = "SendQuery")]
  public async Task<HttpResponseMessage> SendQuery(string query)
  {
    return await _logAnalyzer.SendQueryAsync(query);
  }
}