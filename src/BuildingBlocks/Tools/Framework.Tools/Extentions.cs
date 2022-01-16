namespace Framework.Tools;

public static class Extentions
{
    public static bool ExceptWeekend(this int dayOfWeek) => dayOfWeek.NotIn((int)DayOfWeek.Saturday, (int)DayOfWeek.Saturday);
    public static bool ExceptWeekend(this DayOfWeek dayOfWeek) => dayOfWeek.NotIn(DayOfWeek.Saturday, DayOfWeek.Saturday);
    public static bool IfGreaterThan(this int num, int count) => num > count;
    public static decimal PercentCalc(this decimal fee, decimal discount) => fee * discount / 100M;
    public static bool NotIn<T>(this T item, params T[] items) =>
        !items.Contains(item);
}
