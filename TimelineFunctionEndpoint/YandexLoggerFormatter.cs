using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging.Console;
using System.Text.RegularExpressions;

namespace YandexFunction
{
  public class YandexLoggerFormatter : ConsoleFormatter
  {
    public YandexLoggerFormatter(string name = "YandexLoggerFormatter") : base(name) { }

    public override void Write<TState>(in LogEntry<TState> entry, IExternalScopeProvider provider, TextWriter writer)
    {
      var logLevelString = GetLevelString(entry.LogLevel);
      var message = entry.Formatter(entry.State, entry.Exception);
      message = Regex.Replace(message, "[^!-?a-zA-Zа-яА-Я ]", " ");
      message = message.Replace("\t", " ");
      var logLine = $"{{\"level\": \"{logLevelString}\",\"message\": \"{message}\"}}";

      writer.WriteLine(logLine);
    }

    private static string GetLevelString(LogLevel logLevel)
    {
      return logLevel switch
      {
        LogLevel.Debug => "Debug",
        LogLevel.Warning => "Warn",
        LogLevel.Error => "Error",
        LogLevel.Trace => "Trace",
        LogLevel.Information => "Info",
        _ => "Unknown"
      }; 
    }
  }
}
