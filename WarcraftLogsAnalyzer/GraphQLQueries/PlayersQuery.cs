namespace WarcraftLogsAnalyzer.Query;

public class PlayersQuery : BaseQuery<WLPlayers>
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
