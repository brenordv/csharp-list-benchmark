``` ini

BenchmarkDotNet=v0.13.1, OS=ubuntu 20.04
Intel Core i7-9750H CPU 2.60GHz, 1 CPU, 12 logical and 6 physical cores
.NET SDK=6.0.300
  [Host]     : .NET 6.0.5 (6.0.522.21309), X64 RyuJIT
  DefaultJob : .NET 6.0.5 (6.0.522.21309), X64 RyuJIT


```
|                                     Method |     Mean |    Error |   StdDev | Rank |  Gen 0 | Allocated |
|------------------------------------------- |---------:|---------:|---------:|-----:|-------:|----------:|
|            FindClosestToAvgReadingTime_For | 12.20 μs | 0.234 μs | 0.493 μs |    1 |      - |         - |
| FindClosestToAvgReadingTime_AvgThenOrderBy | 29.83 μs | 0.487 μs | 0.541 μs |    2 | 0.0305 |     272 B |
