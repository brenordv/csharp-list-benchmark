using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Raccoon.Ninja.Demo.Lists.Core;

namespace Raccoon.Ninja.Demo.Cli.Benchmark;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class BenchmarkFastestAvg
{
    /// <summary>
    ///     Just to make things fair and don't waste the benchmark test method with
    /// data creation.
    /// </summary>
    private static readonly IList<Telemetry>[] DataSets = new IList<Telemetry>[5];

    private const int QuantityItemsToCreate = 1000;
    public BenchmarkFastestAvg()
    {
        for (var i = 0; i < DataSets.Length; i++)
        {
            DataSets[i] = TelemetryGenerator.Create(QuantityItemsToCreate).ToList();
        }
    }
    
    
    [Benchmark]
    public void FindFastestReadingTime_LinqSort()
    {
        BenchTestCases.FindFastestReadingTimeLinqSort(DataSets[0]);
    }

    [Benchmark]
    public void FindFastestReadingTime_LinqWhere()
    {
        BenchTestCases.FindFastestReadingTimeLinqWhere(DataSets[1]);
    }
    
    [Benchmark]
    public void FindFastestReadingTime_LinqWhereFixedMin()
    {
        BenchTestCases.FindFastestReadingTimeLinqWhereFixedMin(DataSets[2]);
    }
    
    [Benchmark]
    public void FindFastestReadingTime_ForEach()
    {
        BenchTestCases.FindFastestReadingTimeForEach(DataSets[3]);
    }

    [Benchmark]
    public void FindFastestReadingTime_For()
    {
        BenchTestCases.FindFastestReadingTimeFor(DataSets[4]);
    }
    
    [Benchmark]
    public void FindFastestReadingTime_For_DataControlGroup()
    {
        
        BenchTestCases.FindFastestReadingTimeFor(TelemetryGenerator.Create(QuantityItemsToCreate).ToList());
    }
}