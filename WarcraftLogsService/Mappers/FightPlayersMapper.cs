using WarcraftLogsService.Models;
using Source = WarcraftLogs.ResponseModels;


namespace WarcraftLogsService.Mappers;

public static class PlayerMapper
{
  public static ICollection<FightPlayer> Map(this Source.Players players, Source.Events playersEvents, TimeSpan? startFight = null)
  {
    var events = playersEvents.Data.GroupBy(x => x.SourceID)
      .ToDictionary(group => group.Key, group => group.ToList());

    var dpcPlayers = players.Dps?.Select(player => player.Map(PlayerRole.Dps, events, startFight)) ?? new List<FightPlayer>();
    var healPlayers = players.Healers?.Select(player => player.Map(PlayerRole.Heal, events, startFight)) ?? new List<FightPlayer>();
    var tankPlayers = players.Tanks?.Select(player => player.Map(PlayerRole.Tank, events, startFight)) ?? new List<FightPlayer>();
    
    
    var result = (dpcPlayers.Concat(healPlayers)
                                              .Concat(tankPlayers)
                                              .Where(player => player != null) as IEnumerable<FightPlayer>)
                                    .ToList();
    return result;
  }
  
  public static FightPlayer? Map(this Source.Player player, PlayerRole role, 
    Dictionary<int, List<Source.Event>> playersEvents,
    TimeSpan? startFight = null)
  {
    if (!playersEvents.ContainsKey(player.Id)) return null;
    return new FightPlayer()
    {
      Id = player.Id,
      Name = player.Name,
      Class = player.Type,
      Spec = player.Specs.First().Spec,
      Role = role,
      Abilities = playersEvents[player.Id].Map(startFight)
    };
  }
}