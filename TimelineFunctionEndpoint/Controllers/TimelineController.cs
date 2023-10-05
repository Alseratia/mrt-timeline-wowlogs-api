using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Timeline;

/// <summary>
/// Controller for the client part of the application. Returns TimelineStoreState from this log
/// </summary>

public class TimelineController : Controller
{
  private readonly TimelineService _timelineService;
  public TimelineController(TimelineService timelineService)
    => _timelineService = timelineService;


  /// <summary>
  /// Create timeline store from this log and fight number.
  /// </summary>
  [HttpGet("timeline/{code}/{fightId}", Name = "get-timeline-store")]
  public async Task<ActionResult> GetTimelineStore([StringLength(16, MinimumLength = 16)] string code, uint fightId)
  {
    return await _timelineService.GetTimelineStore(code, fightId);
  }

  /// <summary>
  /// Create timeline store from this log and last fight.
  /// </summary>
  [HttpGet("timeline/{code}/last", Name = "get-timeline-store-last")]
  public async Task<ActionResult> GetTimelineStore([StringLength(16, MinimumLength = 16)] string code)
  {
    return await _timelineService.GetTimelineStoreLast(code);
  }
}
