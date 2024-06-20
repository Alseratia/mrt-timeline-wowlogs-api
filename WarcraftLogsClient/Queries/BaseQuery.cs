namespace WarcraftLogs.Query;

public abstract class BaseQuery<TReturnType> where TReturnType : class
{
  protected string Query { get; init; } = null!;
  public override string ToString() => Query;
}
