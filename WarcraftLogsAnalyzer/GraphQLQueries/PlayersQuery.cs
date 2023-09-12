namespace WarcraftLogsAnalyzer.Models;

public class PlayersQuery : AbstractQuery<WLPlayers>
{
  public PlayersQuery(string code, int fightId)
  {
    Query = $@"
          query {{
            reportData {{
              report(code: ""{code}"") {{
                playerDetails(fightIDs: {fightId})
              }}
            }}
          }}
      ";
  }
}
