using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Raccoon.Ninja.Demo.Lists.Core;

namespace Raccoon.Ninja.Demo.Cli.Benchmark;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class BenchmarkClosestToAvg
{
    /// <summary>
    ///     Just to make things fair and don't waste the benchmark test method with
    /// data creation.
    /// </summary>
    private static readonly IList<Telemetry>[] DataSets = new IList<Telemetry>[2];
    private const int QuantityItemsToCreate = 1000;
    public BenchmarkClosestToAvg()
    {
        for (var i = 0; i < DataSets.Length; i++)
        {
            DataSets[i] = TelemetryGenerator.Create(QuantityItemsToCreate).ToList();
        }
    }

    [Benchmark]
    public void FindClosestToAvgReadingTime_AvgThenOrderBy()
    {
        BenchTestCases.FindClosestToAvgReadingTimeAvgThenOrderBy(DataSets[0]);
    }

    [Benchmark]
    public void FindClosestToAvgReadingTime_For()
    {
        BenchTestCases.FindClosestToAvgReadingTimeFor(DataSets[1]);
    }
    
    [Benchmark]
    public void FindClosestToAvgReadingTime_For_DataControlGroup()
    {
        BenchTestCases.FindClosestToAvgReadingTimeFor(TelemetryGenerator.Create(QuantityItemsToCreate).ToList());
    }
}