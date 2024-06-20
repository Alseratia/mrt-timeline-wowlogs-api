namespace Shared;

public static class EnumMapper
{
  public static TTarget? Map<TTarget>(this Enum sourceEnumValue) where TTarget : Enum
  {
    Enum.TryParse(typeof(TTarget), sourceEnumValue.ToString(), true, out var newValue);
    return (TTarget?)newValue;
  }
}