using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Controller for the client part of the application. Returns TimelineStoreState from this log
/// </summary>
public class TimelineController : Controller
{
  private readonly TimelineService _timelineService;
  public TimelineController(TimelineService timelineService)
    => _timelineService = timelineService;

  /// <summary>
  /// Create timeline store from this log.
  /// </summary>
  [HttpGet("timeline/{code}/{fightId}", Name = "get-timeline-store")]
  public async Task<ActionResult> GetTimelineStore(string code, int fightId)
  {
    return await _timelineService.GetTimelineStore(code, fightId);
  }
}