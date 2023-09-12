namespace WarcraftLogsAnalyzer.Models;

public static class WarcraftLogsAnalyzerModelsExtentions
{
  public static List<WLEvent> CalcDurationAndNumber(this List<WLEvent> events)
  {
    events.RemoveAll(x => x.Type == WLEventType.applybuffstack || x.Type == WLEventType.removebuffstack ||
                          x.Type == WLEventType.applydebuffstack || x.Type == WLEventType.removedebuffstack ||
                          x.Type == WLEventType.refreshdebuff || x.Type == WLEventType.refreshbuff);

    var groups = events.GroupBy(ability => ability.AbilityGameId);
    foreach (var group in groups)
    {
      WLEvent? startEvent = null;
      var index = 1;

      foreach (var ability in group)
      {
        // CastNumber
        ability.CastNumber = index;

        // Duration
        if (ability.Type == WLEventType.begincast || ability.Type == WLEventType.applybuff)
        {
          if (startEvent != null) Console.WriteLine($"Error ability duration for {ability.AbilityGameId}. Pair not found"); // TODO log
          startEvent = ability;
          continue;
        }

        if (startEvent != null && (ability.Type == WLEventType.cast || ability.Type == WLEventType.removebuff))
        {
          var duration = Convert.ToInt32((ability.Timestamp - startEvent.Timestamp) / 1000.0);
          startEvent.Duration = duration;
          ability.Duration = duration;
          startEvent = null;
        }

        index++;
      }
    }
    return events;
  }
}