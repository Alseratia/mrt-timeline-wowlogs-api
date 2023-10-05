namespace WarcraftLogsAnalyzer.Query;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class FilterParameterNameAttribute : Attribute
{
  public string Name { get; }

  public FilterParameterNameAttribute(string name)
  {
    Name = name;
  }
}
