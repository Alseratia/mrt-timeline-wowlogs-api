using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;

namespace Timeline
{
  public class YandexLoggerFormatter : ConsoleFormatter
  {
    public YandexLoggerFormatter(string name = "YandexLoggerFormatter") : base(name) { }

    public override void Write<TState>(in LogEntry<TState> entry, IExternalScopeProvider provider, TextWriter writer)
    {
      var logLevelString = entry.LogLevel.ToString().ToUpperInvariant();
      var message = entry.Formatter(entry.State, entry.Exception);

      var logLine = @$"
      {{
        ""level"" = {logLevelString},
        ""message"": ""{message}""
      }}";

      writer.WriteLine(logLine.Replace('\n', '\r'));
    }
  }
}
