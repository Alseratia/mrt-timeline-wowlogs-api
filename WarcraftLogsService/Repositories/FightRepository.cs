using WarcraftLogs;
using WarcraftLogs.Query;
using WarcraftLogsService.Mappers;
using WarcraftLogsService.Models;

namespace WarcraftLogsService.Repositories;

public class FightRepository
{
  private readonly WarcraftLogsClient _client;
  
  public FightRepository(WarcraftLogsClient client)
    => (_client) = (client);
  
  public async Task<Fight?> GetFight(string code, uint fightId, 
    ICollection<int> filterBossAbilities, ICollection<int> filterPlayersAbilities, int? sourseId = null)
  {
    var fight = (await _client.SendQuery(new ReportQuery(code, fightId,
      filterBossAbilities, filterPlayersAbilities, sourseId))).Content!.Data!;

    return fight.Map();
  }

  public  async Task<Fight?> GetLastFight(string code, ICollection<int> filterBossAbilities, 
    ICollection<int> filterPlayersAbilities, int? sourseId = null)
  {
    var response = await _client.SendQuery(new FightsQuery(code));
    var fights = response.Content!.Data!.ReportData.Report!.Fights;
    var lastBossFightId = (uint)(Array.FindLastIndex(fights, x => x.Difficulty != null) + 1);
    return await GetFight(code, lastBossFightId, filterBossAbilities, filterPlayersAbilities, sourseId);
  }
}