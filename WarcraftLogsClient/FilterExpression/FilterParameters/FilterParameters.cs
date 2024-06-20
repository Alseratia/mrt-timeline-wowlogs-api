namespace WarcraftLogs;

/// <summary>
/// A class containing supported fields from which a boolean expression can be composed, 
/// the type of this field and its name when translated into a string.
/// More parameters: https://articles.warcraftlogs.com/help/intro-to-expressions
/// </summary>
public abstract class FilterParameters
{
  [FilterParameterName("ability.id")]
  public int AbilityId { get; private set; }
  [FilterParameterName("ability.name")]
  public string AbilityName { get; private set; } = null!;
  [FilterParameterName("type")]
  public EventType Type { get; private set; }
  [FilterParameterName("source.type")]
  public SourceType SourceType { get; private set; }
  [FilterParameterName("inCategory('casts')")]
  public bool InCategoryCasts { get; private set; }
  [FilterParameterName("inCategory('auras')")]
  public bool InCategoryAuras { get; private set; }
}
