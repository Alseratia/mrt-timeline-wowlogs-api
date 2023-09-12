using Microsoft.AspNetCore.Mvc;

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
  [HttpPost("upload-log/{code}/{fightId}", Name = "upload-log")]
  public async Task<ActionResult> UploadLog(string code, int fightId)
  {
    return await _updateDBService.UploadLog(code, fightId);
  }

  /// <summary>
  /// Upload all raid bosses only
  /// </summary>
  [HttpPost("upload-raid-bosses/{raidName}", Name = "upload-raid-bosses")]
  public async Task<ActionResult> UploadRaidBosses(string raidName)
  {
    return await _updateDBService.UploadRaidBosses(raidName);
  }
}