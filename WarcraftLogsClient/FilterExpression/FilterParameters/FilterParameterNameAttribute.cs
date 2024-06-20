namespace WarcraftLogs;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class FilterParameterNameAttribute : Attribute
{
  public string Name { get; init; }
  public FilterParameterNameAttribute(string name) => Name = name;
}
