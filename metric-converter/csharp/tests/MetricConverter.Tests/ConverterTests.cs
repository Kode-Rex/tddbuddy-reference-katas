using FluentAssertions;
using Xunit;

namespace MetricConverter.Tests;

public class ConverterTests
{
    [Fact]
    public void Converts_kilometers_to_miles()
    {
        Converter.Convert(1, Unit.Kilometers, Unit.Miles)
            .Should().BeApproximately(0.621371, 1e-9);
    }

    [Fact]
    public void Converts_kilometers_to_miles_for_a_larger_value()
    {
        Converter.Convert(10, Unit.Kilometers, Unit.Miles)
            .Should().BeApproximately(6.21371, 1e-9);
    }

    [Fact]
    public void Converts_zero_kilometers_to_zero_miles()
    {
        Converter.Convert(0, Unit.Kilometers, Unit.Miles).Should().Be(0);
    }

    [Fact]
    public void Converts_celsius_to_fahrenheit_for_freezing_point()
    {
        Converter.Convert(0, Unit.Celsius, Unit.Fahrenheit).Should().Be(32);
    }

    [Fact]
    public void Converts_celsius_to_fahrenheit_for_body_temperature()
    {
        Converter.Convert(30, Unit.Celsius, Unit.Fahrenheit).Should().Be(86);
    }

    [Fact]
    public void Converts_negative_celsius_to_fahrenheit()
    {
        Converter.Convert(-40, Unit.Celsius, Unit.Fahrenheit).Should().Be(-40);
    }

    [Fact]
    public void Converts_kilograms_to_pounds()
    {
        Converter.Convert(5, Unit.Kilograms, Unit.Pounds)
            .Should().BeApproximately(11.0231131, 1e-6);
    }

    [Fact]
    public void Converts_zero_kilograms_to_zero_pounds()
    {
        Converter.Convert(0, Unit.Kilograms, Unit.Pounds).Should().Be(0);
    }

    [Fact]
    public void Converts_liters_to_us_gallons()
    {
        Converter.Convert(3.785411784, Unit.Liters, Unit.UsGallons)
            .Should().BeApproximately(1, 1e-9);
    }

    [Fact]
    public void Converts_liters_to_uk_gallons()
    {
        Converter.Convert(4.54609, Unit.Liters, Unit.UkGallons)
            .Should().BeApproximately(1, 1e-9);
    }

    [Fact]
    public void Converts_ten_liters_to_us_gallons()
    {
        Converter.Convert(10, Unit.Liters, Unit.UsGallons)
            .Should().BeApproximately(2.641720524, 1e-6);
    }

    [Fact]
    public void Converts_ten_liters_to_uk_gallons()
    {
        Converter.Convert(10, Unit.Liters, Unit.UkGallons)
            .Should().BeApproximately(2.199692483, 1e-6);
    }

    [Fact]
    public void Rejects_an_unsupported_conversion_pair()
    {
        var act = () => Converter.Convert(1, Unit.Kilometers, Unit.Fahrenheit);
        act.Should().Throw<ArgumentException>()
            .WithMessage("Unsupported conversion: Kilometers to Fahrenheit");
    }
}
