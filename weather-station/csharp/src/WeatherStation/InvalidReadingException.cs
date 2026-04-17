namespace WeatherStation;

public class InvalidReadingException : Exception
{
    public InvalidReadingException(string message) : base(message) { }
}
