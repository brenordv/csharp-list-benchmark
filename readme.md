# Benchmark for common list operations
This repo has a simple benchmark to showcase the performance of various ways of looking through lists in C#.

I know that the benchmark result is pretty obvious, but it is nice to see the actual performance comparison. 
Also, this project can be easily expanded to include more complex tests. 

## Solution Structure
- Raccoon.Ninja.Demo.Cli.**Benchmark**: Console Application that runs the Benchmarks.
- Raccoon.Ninja.Demo.Cli.**Playground**: Console Application to play around with the functionalities.
- Raccoon.Ninja.Demo.**Lists.Core**: This is where the base logic is.

> Note: This is a super simple project, so I didn't bother creating organized folder structures.

## Target entity
As a sample entity, i created `Telemetry` with a couple properties. It's not really based on anything, just to vary
a bit from the usual 'person entity'.

```c#
public record Telemetry(
    Guid CorrelationId,
    string Tag,
    TimeSpan AvgReadingTime,
    DateTime ReadAt,
    double Value,
    bool IsCalibrationData
);
```

### Test data generation
To generate test data, I used Bogus. All data is random and the AvgReadingTime ranges from 42ms to 3600ms.

# Benchmark
I created two benchmark scenarios:
- Find the telemetry data with lowest AvgReadingTime.
- Find the telemetry data where the AvgReadingTime is closest to the list average.

## Lowest AvgReadingTime
I created 5 tests for this scenario:

### Using OrderBy then First
```c#
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
```

### Using First + Min (inside closure)
```c# 
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
```

### Using First + Min (outside closure)
```c#
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
```

### Using foreach
```c#
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
```

### Using for loop
```c#
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
```

## Telemetry with the AvgReadingTime closest to the list average.
I created 2 tests for this scenario:

For each scenario, I created created a test case where I generate the data inside the benchmark method. This is just to check the impact of doing that. For the purposes of analyzing the performance of the operations, you can disregard that result.

### Using Average then OrderBy
```c#
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
```

### Using for loop
```c#
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
```

# Benchmark Results
## Lowest AvgReadingTime
|                                      Method |         Mean |       Error |      StdDev | Rank |    Gen 0 |   Gen 1 |   Allocated |
|-------------------------------------------- |-------------:|------------:|------------:|-----:|---------:|--------:|------------:|
|                  FindFastestReadingTime_For |     5.160 us |   0.1223 us |   0.3528 us |    1 |        - |       - |           - |
|              FindFastestReadingTime_ForEach |     9.843 us |   0.1940 us |   0.4497 us |    2 |        - |       - |        40 B |
|    FindFastestReadingTime_LinqWhereFixedMin |    13.708 us |   0.2723 us |   0.5802 us |    3 |   0.0153 |       - |       168 B |
|             FindFastestReadingTime_LinqSort |    16.220 us |   0.3213 us |   0.7510 us |    4 |        - |       - |       144 B |
| FindFastestReadingTime_For_DataControlGroup | 2,073.318 us |  41.0719 us |  98.4057 us |    5 | 160.1563 | 62.5000 | 1,011,367 B |
|            FindFastestReadingTime_LinqWhere | 7,502.086 us | 147.1028 us | 307.0583 us |    6 |        - |       - |    27,734 B |


## Telemetry with the AvgReadingTime closest to the list average.
|                                           Method |        Mean |     Error |     StdDev | Rank |    Gen 0 |   Gen 1 |   Allocated |
|------------------------------------------------- |------------:|----------:|-----------:|-----:|---------:|--------:|------------:|
|                  FindClosestToAvgReadingTime_For |    12.52 us |  0.340 us |   1.004 us |    1 |        - |       - |           - |
|       FindClosestToAvgReadingTime_AvgThenOrderBy |    33.07 us |  1.017 us |   2.933 us |    2 |        - |       - |       272 B |
| FindClosestToAvgReadingTime_For_DataControlGroup | 2,167.03 us | 62.924 us | 180.542 us |    3 | 160.1563 | 62.5000 | 1,011,318 B |


# How to
## Build and run
(from solution folder)

### Windows
```shell
dotnet build Raccoon.Ninja.Demo.Cli.Benchmark\Raccoon.Ninja.Demo.Cli.Benchmark.csproj -c Release
.\Raccoon.Ninja.Demo.Cli.Benchmark\bin\Release\net6.0\Raccoon.Ninja.Demo.Cli.Benchmark.exe
```

### Linux
```shell
dotnet build Raccoon.Ninja.Demo.Cli.Benchmark/Raccoon.Ninja.Demo.Cli.Benchmark.csproj -c Release
./Raccoon.Ninja.Demo.Cli.Benchmark/bin/Release/net6.0/Raccoon.Ninja.Demo.Cli.Benchmark
```

