public static class DictionaryExtensions
{
  public static void RemoveAll<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, 
    Func<TValue, bool> predicate)
  {
    var keys = dictionary.Keys.Where(k => predicate(dictionary[k])).ToList();
    foreach (var key in keys)
    {
      dictionary.Remove(key);
    }
  }
}