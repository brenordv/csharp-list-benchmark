// See https://aka.ms/new-console-template for more information

using Raccoon.Ninja.Demo.Lists.Core;

Console.WriteLine("Hello, World!");

DataGeneration();
FastestTelemetry();
ClosestToAvg();


void DataGeneration()
{
    Console.WriteLine("----------------| Data Generation");
    TelemetryGenerator.Create(10).SortByAvgReadingTime().Print();
}

void FastestTelemetry()
{
    Console.WriteLine("----------------| Fastest Telemetry");
    var data = TelemetryGenerator.Create(10).ToList();
    var fastest1 = BenchTestCases.FindFastestReadingTimeFor(data);
    var fastest2 = BenchTestCases.FindFastestReadingTimeForEach(data);
    var fastest3 = BenchTestCases.FindFastestReadingTimeLinqSort(data);
    var fastest4 = BenchTestCases.FindFastestReadingTimeLinqWhere(data);
    var fastest5 = BenchTestCases.FindFastestReadingTimeLinqWhereFixedMin(data);
    Console.WriteLine($"FindFastestReadingTimeFor: {fastest1}");
    Console.WriteLine($"FindFastestReadingTimeForEach: {fastest2}");
    Console.WriteLine($"FindFastestReadingTimeLinqSort: {fastest3}");
    Console.WriteLine($"FindFastestReadingTimeLinqWhere: {fastest4}");
    Console.WriteLine($"FindFastestReadingTimeLinqWhereFixedMin: {fastest5}");
    Console.WriteLine("--------------------");
    data.SortByAvgReadingTime().Print();
}

void ClosestToAvg()
{
    Console.WriteLine("----------------| Closest to Avg");
    var data2 = TelemetryGenerator.Create(10).ToList();
    var avg = data2.Average(d => d.AvgReadingTime.TotalMilliseconds);
    var closestAvg1 = BenchTestCases.FindClosestToAvgReadingTimeAvgThenOrderBy(data2);
    var closestAvg2 = BenchTestCases.FindClosestToAvgReadingTimeFor(data2);

    Console.WriteLine(
        $"Average: {avg} - {TimeSpan.FromMilliseconds(avg)}| c1: {closestAvg1.AvgReadingTime} | c2: {closestAvg2.AvgReadingTime}");
    Console.WriteLine(closestAvg1);
    Console.WriteLine(closestAvg2);
    Console.WriteLine("--------------------");
    data2.SortByAvgReadingTime().Print();
}