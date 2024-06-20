using System.ComponentModel.DataAnnotations;
using Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/timeline/")]
public class TimelineController : ControllerBase
{
  public TimelineController() {}
  
  [HttpGet("{code}/{fightId}", Name = "get-timeline-store")]
  public async Task<ActionResult> GetTimelineStoreState([StringLength(16, MinimumLength = 16)] string code, uint fightId, 
    [FromQuery(Name="source")] int? sourceId, [FromServices] GetTimelineFromLog useCase)
  {
    return Ok(await useCase.Execute(code, fightId, sourceId));
  }
  
  [HttpGet("{code}/last", Name = "get-timeline-store-last")]
  public async Task<IActionResult> GetTimelineStoreState([StringLength(16, MinimumLength = 16)] string code, 
    [FromQuery(Name="source")] int? sourceId, [FromServices] GetTimelineFromLog useCase)
  {
    return Ok(await useCase.Execute(code, sourceId));
  }
}