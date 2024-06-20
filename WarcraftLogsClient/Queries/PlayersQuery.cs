using WarcraftLogs.ResponseModels;

namespace WarcraftLogs.Query;

public class PlayersQuery : BaseQuery<PlayersData>
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
