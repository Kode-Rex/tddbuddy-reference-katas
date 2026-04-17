namespace WeatherStation;

public readonly record struct Reading(
    decimal Temperature,
    decimal Humidity,
    decimal WindSpeed,
    DateTime Timestamp);
