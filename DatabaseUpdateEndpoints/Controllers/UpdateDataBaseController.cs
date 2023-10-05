using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TimelineDatabaseContext;

/// <summary>
/// Controller for updating the database
/// </summary>
public class UpdateDatabaseController : Controller
{
  private readonly UpdateDBService _updateDBService;
  public UpdateDatabaseController(UpdateDBService updateDBService)
  {
    _updateDBService = updateDBService;
  }

  /// <summary>
  /// Upload all log fight data and replace old events data.
  /// </summary>
  [HttpGet("dashboard/boss/{code}/{fightId}", Name = "GetBoss")]
  public async Task<ActionResult> GetBoss(string code, string fightId)
  {
    return await _updateDBService.GetBossFromLog(code, fightId);
  }

  /// <summary>
  /// Upload all log fight data and replace old events data.
  /// </summary>
  [HttpPost("dashboard/boss/complete", Name = "GetCompleteBoss")]
  public async Task<ActionResult> GetCompleteBoss([FromBody] RootPartialBossDto bossDto)
  {
    return await _updateDBService.GetCompletedBoss(bossDto.ReportCode, bossDto.FightNumber, bossDto.Boss, bossDto.PartialStages);
  }

  [HttpPost("dashboard/boss/create", Name = "CreateNewBoss")]
  public async Task<ActionResult> SaveBoss([FromBody] RootFullBossDto bossDto)
  {
    return await _updateDBService.SaveBoss(bossDto.Boss);
  }
}