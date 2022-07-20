namespace Raccoon.Ninja.Demo.Lists.Core;

public record Telemetry(
    Guid CorrelationId,
    string Tag,
    TimeSpan AvgReadingTime,
    DateTime ReadAt,
    double Value,
    bool IsCalibrationData
);