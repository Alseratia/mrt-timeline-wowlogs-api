using Shared;
using WarcraftLogsService.Models;
using Source = WarcraftLogs.ResponseModels;

namespace WarcraftLogsService.Mappers;

public static class FightConverter
{
  public static Fight? Map(this Source.ReportInfo reportInfo)
  {
    var report = reportInfo.ReportData.Report;

    var fight = report?.Fights.FirstOrDefault();
    if (fight == null) return null;
    
    var players = report?.PlayerDetails.Data.PlayerDetails;
    if (players == null) return null;

    var difficulty = fight.Difficulty.Map<Difficulty>();
    if (difficulty == null) return null;
    
    return new Fight()
    {
      BossId = fight.EncounterID,
      BossName = fight.Name,
      ZoneName = fight.GameZone!.Name,
      StartTime = fight.StartTime,
      EndTime = fight.EndTime,
      Difficulty = difficulty,
      Players = players.Map(report!.PlayersEvents, fight.StartTime),
      BossAbilities = report.BossEvents.Data.Map(fight.StartTime)
    };
  }
}