namespace MetricConverter;

public static class Converter
{
    public static double Convert(double value, Unit from, Unit to) =>
        (from, to) switch
        {
            (Unit.Kilometers, Unit.Miles) => value * 0.621371,
            (Unit.Celsius, Unit.Fahrenheit) => (value * 1.8) + 32,
            (Unit.Kilograms, Unit.Pounds) => value / 0.45359237,
            (Unit.Liters, Unit.UsGallons) => value / 3.785411784,
            (Unit.Liters, Unit.UkGallons) => value / 4.54609,
            _ => throw new ArgumentException(
                $"Unsupported conversion: {from} to {to}"),
        };
}
