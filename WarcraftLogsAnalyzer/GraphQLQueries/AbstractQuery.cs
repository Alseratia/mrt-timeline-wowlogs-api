namespace WarcraftLogsAnalyzer.Models;

public abstract class AbstractQuery<T> where T : class
{
  protected virtual string Query { get; set; } = "";
  public static implicit operator string(AbstractQuery<T> query) => query.Query;
}
