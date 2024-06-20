using WarcraftLogs.ResponseModels;

namespace WarcraftLogs.Query;

public class AbilityQuery : BaseQuery<AbilityData>
{
  public AbilityQuery(int id)
  {
    Query = $@"
      query {{
        gameData {{
          ability(id: {id}) {{
            id 
            name 
            icon
          }}
        }}
      }}";
  }
}
