namespace WeatherStation;

public class NoReadingsException : Exception
{
    public NoReadingsException() : base("Cannot compute statistics with no readings.") { }
}
