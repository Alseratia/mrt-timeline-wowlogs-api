namespace WarcraftLogsAnalyzer.Query;

public class AbilityQuery : BaseQuery<WLAbility>
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
