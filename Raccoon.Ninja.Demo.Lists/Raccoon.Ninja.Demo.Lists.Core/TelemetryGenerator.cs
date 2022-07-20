using Bogus;

namespace Raccoon.Ninja.Demo.Lists.Core;

public static class TelemetryGenerator
{
    public static IEnumerable<Telemetry> Create(int quantity)
    {
        const int minMsRandRange = 42;
        const int maxMsRandRange = 3600;
        const float calibrationTrueWeight = 0.4f;
        
        var testTelemetry = new Faker<Telemetry>();
        return testTelemetry
            .CustomInstantiator(f => new Telemetry(
                Guid.NewGuid(),
                $"{f.Hacker.Noun()}_{f.Hacker.Noun()}",
                TimeSpan.FromMilliseconds(f.Random.Int(minMsRandRange, maxMsRandRange)),
                DateTime.UtcNow.AddMilliseconds(-f.Random.Int(minMsRandRange, maxMsRandRange)),
                f.Random.Double(),
                f.Random.Bool(calibrationTrueWeight)
            ))
            .Generate(quantity);
    }
}