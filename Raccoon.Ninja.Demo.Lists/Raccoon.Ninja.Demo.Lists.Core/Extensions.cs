namespace Raccoon.Ninja.Demo.Lists.Core;

public static class Extensions
{
    public static IEnumerable<Telemetry> SortByReadAt(this IEnumerable<Telemetry> list, bool ascending = true)
    {
        var data = list.ToList();
        return ascending 
            ? data.OrderBy(d => d.ReadAt) 
            : data.OrderByDescending(d => d.ReadAt);
    }
    
    public static IEnumerable<Telemetry> SortByAvgReadingTime(this IEnumerable<Telemetry> list, bool ascending = true)
    {
        var data = list.ToList();
        return ascending 
            ? data.OrderBy(d => d.AvgReadingTime) 
            : data.OrderByDescending(d => d.AvgReadingTime);
    }

    public static void Print(this IEnumerable<Telemetry> list)
    {
        foreach (var telemetry in list)
        {
            Console.WriteLine(telemetry);
        }
    }
}