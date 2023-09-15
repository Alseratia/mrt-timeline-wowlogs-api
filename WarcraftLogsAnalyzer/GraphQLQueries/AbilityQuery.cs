namespace WarcraftLogs.Query;

public class AbilityQuery : AbstractQuery<WLAbility>
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
