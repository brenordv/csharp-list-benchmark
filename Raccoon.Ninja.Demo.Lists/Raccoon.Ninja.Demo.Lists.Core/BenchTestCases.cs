namespace Raccoon.Ninja.Demo.Lists.Core;

public static class BenchTestCases
{
    /// <summary>
    ///     Returns the <see cref="Telemetry">Telemetry</see> with lowest AvgReadingTime.
    /// </summary>
    /// <remarks>
    ///     Uses OrderBy and then gets the first item in the list. 
    /// </remarks>
    /// <param name="data">List of Telemetry data to be analyzed</param>
    /// <returns>Telemetry with the fastest AvgReadingTime</returns>
    public static Telemetry FindFastestReadingTimeLinqSort(IList<Telemetry> data)
    {
        return data.OrderBy(d => d.AvgReadingTime).First();
    }
    
    /// <summary>
    ///     Returns the <see cref="Telemetry">Telemetry</see> with lowest AvgReadingTime.
    /// </summary>
    /// <remarks>
    ///     Uses First where AvgReadingTime equals the minimum value.
    ///     The definition of the minimum value is made inside the linq expression. 
    /// </remarks>
    /// <param name="data">List of Telemetry data to be analyzed</param>
    /// <returns>Telemetry with the fastest AvgReadingTime</returns>
    public static Telemetry FindFastestReadingTimeLinqWhere(IList<Telemetry> data)
    {
        return data.First(d => d.AvgReadingTime == data.Min(d1 => d1.AvgReadingTime));
    }
    
    /// <summary>
    ///     Returns the <see cref="Telemetry">Telemetry</see> with lowest AvgReadingTime.
    /// </summary>
    /// <remarks>
    ///     Uses First where AvgReadingTime equals the minimum value.
    ///     The definition of the minimum value is made only once, outside the linq expression. 
    /// </remarks>
    /// <param name="data">List of Telemetry data to be analyzed</param>
    /// <returns>Telemetry with the fastest AvgReadingTime</returns>
    public static Telemetry FindFastestReadingTimeLinqWhereFixedMin(IList<Telemetry> data)
    {
        var min = data.Min(d1 => d1.AvgReadingTime);
        return data.First(d => d.AvgReadingTime == min);
    }
    
    /// <summary>
    ///     Returns the <see cref="Telemetry">Telemetry</see> with lowest AvgReadingTime.
    /// </summary>
    /// <remarks>
    ///     Uses a foreach loop and checks every AvgReadingTime to find the lowest one.
    /// </remarks>
    /// <param name="data">List of Telemetry data to be analyzed</param>
    /// <returns>Telemetry with the fastest AvgReadingTime</returns>
    public static Telemetry FindFastestReadingTimeForEach(IList<Telemetry> data)
    {
        var min = TimeSpan.MaxValue;
        Telemetry minTelemetry = null;
        foreach (var telemetry in data)
        {
            if (telemetry.AvgReadingTime >= min) continue;
            min = telemetry.AvgReadingTime;
            minTelemetry = telemetry;
        }

        return minTelemetry;
    }
    
    /// <summary>
    ///     Returns the <see cref="Telemetry">Telemetry</see> with lowest AvgReadingTime.
    /// </summary>
    /// <remarks>
    ///     Uses a for loop and checks every AvgReadingTime to find the lowest one.
    /// </remarks>
    /// <param name="data">List of Telemetry data to be analyzed</param>
    /// <returns>Telemetry with the fastest AvgReadingTime</returns>
    public static Telemetry FindFastestReadingTimeFor(IList<Telemetry> data)
    {
        var min = TimeSpan.MaxValue;
        Telemetry minTelemetry = null;
        for (var i = 0; i < data.Count; i++)
        {
            if (data[i].AvgReadingTime >= min) continue;
            min = data[i].AvgReadingTime;
            minTelemetry = data[i];
        }

        return minTelemetry;
    }

    /// <summary>
    ///     Returns the <see cref="Telemetry">Telemetry</see> where the AvgReadingTime is closest to the average of the
    /// list.
    /// </summary>
    /// <remarks>
    ///     This is done in three steps:
    ///      1st: Linq Average to find the AvgReadingTime
    ///      2nd: Linq OrderBy the difference between AvgReadingTime and the list average.
    ///      3rd: Gets the first item (after ordering), since it will be the closest to the average.
    /// </remarks>
    /// <param name="data">List of Telemetry data to be analyzed</param>
    /// <returns>Telemetry where AvgReadingTime is closest to the list average</returns>
    public static Telemetry FindClosestToAvgReadingTimeAvgThenOrderBy(IList<Telemetry> data)
    {
        var avg = data.Average(d => d.AvgReadingTime.TotalMilliseconds);
        return data.OrderBy(d => Math.Abs(d.AvgReadingTime.TotalMilliseconds - avg)).First();
    }

    /// <summary>
    ///     Returns the <see cref="Telemetry">Telemetry</see> where the AvgReadingTime is closest to the average of the
    /// list.
    /// </summary>
    /// <remarks>
    ///     This is done in three steps:
    ///      1st: Sums the AvgReadingTime using a for loop.
    ///      2nd: Using the sum from the previous step, calculates the average of the list.
    ///      3rd: As a starting point, assume that the first item of the list is the right one.
    ///      4th: Using a for loop that starts from the second element in the list, checks which element is the closest
    /// to the list average.
    /// </remarks>
    /// <param name="data">List of Telemetry data to be analyzed</param>
    /// <returns>Telemetry where AvgReadingTime is closest to the list average</returns>
    public static Telemetry FindClosestToAvgReadingTimeFor(IList<Telemetry> data)
    {
        double sum = 0;
        double avg;
        
        for (var i = 0; i < data.Count; i++)
        {
            sum += data[i].AvgReadingTime.TotalMilliseconds;
        }

        avg = sum / data.Count;

        var closest = Math.Abs(data[0].AvgReadingTime.TotalMilliseconds - avg);
        var closestTelemetry = data[0];
        
        for (var i = 1; i < data.Count; i++)
        {
            var totalMs = data[i].AvgReadingTime.TotalMilliseconds;
            var diff = Math.Abs(totalMs - avg);
            if (diff >= closest) continue;
            closest = diff;
            closestTelemetry = data[i];
        }
        
        return closestTelemetry;
    }
}