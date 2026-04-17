namespace WeatherStation;

public readonly record struct Statistics(
    decimal MinTemperature,
    decimal MaxTemperature,
    decimal AvgTemperature,
    decimal MinHumidity,
    decimal MaxHumidity,
    decimal AvgHumidity,
    decimal MaxWindSpeed,
    decimal AvgWindSpeed);
