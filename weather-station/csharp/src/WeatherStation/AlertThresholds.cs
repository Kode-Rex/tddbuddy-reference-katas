namespace WeatherStation;

public readonly record struct AlertThresholds(
    decimal? HighTemperatureCeiling = null,
    decimal? LowTemperatureFloor = null,
    decimal? HighWindSpeedLimit = null);
