namespace WarcraftLogsAnalyzer.Query;

public abstract class BaseQuery<T> where T : class
{
  protected virtual string Query { get; set; } = "";
  public static implicit operator string(BaseQuery<T> query) => query.Query;
}
