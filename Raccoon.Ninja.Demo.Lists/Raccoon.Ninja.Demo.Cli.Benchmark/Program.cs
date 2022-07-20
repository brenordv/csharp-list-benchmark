using BenchmarkDotNet.Running;
using Raccoon.Ninja.Demo.Cli.Benchmark;

Console.WriteLine("Hello, World!");
//BenchmarkRunner.Run<BenchmarkFastestAvg>();
BenchmarkRunner.Run<BenchmarkClosestToAvg>();