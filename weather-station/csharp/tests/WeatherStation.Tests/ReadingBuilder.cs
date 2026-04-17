namespace WeatherStation.Tests;

public class ReadingBuilder
{
    private decimal _temperature = 20m;
    private decimal _humidity = 50m;
    private decimal _windSpeed = 10m;

    public ReadingBuilder WithTemperature(decimal temperature) { _temperature = temperature; return this; }
    public ReadingBuilder WithHumidity(decimal humidity) { _humidity = humidity; return this; }
    public ReadingBuilder WithWindSpeed(decimal windSpeed) { _windSpeed = windSpeed; return this; }

    public (decimal Temperature, decimal Humidity, decimal WindSpeed) Build()
    {
        return (_temperature, _humidity, _windSpeed);
    }
}
