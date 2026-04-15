import pytest

from metric_converter import Unit, convert


def test_converts_kilometers_to_miles():
    assert convert(1, Unit.KILOMETERS, Unit.MILES) == pytest.approx(0.621371)


def test_converts_kilometers_to_miles_for_a_larger_value():
    assert convert(10, Unit.KILOMETERS, Unit.MILES) == pytest.approx(6.21371)


def test_converts_zero_kilometers_to_zero_miles():
    assert convert(0, Unit.KILOMETERS, Unit.MILES) == 0


def test_converts_celsius_to_fahrenheit_for_freezing_point():
    assert convert(0, Unit.CELSIUS, Unit.FAHRENHEIT) == 32


def test_converts_celsius_to_fahrenheit_for_body_temperature():
    assert convert(30, Unit.CELSIUS, Unit.FAHRENHEIT) == 86


def test_converts_negative_celsius_to_fahrenheit():
    assert convert(-40, Unit.CELSIUS, Unit.FAHRENHEIT) == -40


def test_converts_kilograms_to_pounds():
    assert convert(5, Unit.KILOGRAMS, Unit.POUNDS) == pytest.approx(11.0231131)


def test_converts_zero_kilograms_to_zero_pounds():
    assert convert(0, Unit.KILOGRAMS, Unit.POUNDS) == 0


def test_converts_liters_to_us_gallons():
    assert convert(3.785411784, Unit.LITERS, Unit.US_GALLONS) == pytest.approx(1)


def test_converts_liters_to_uk_gallons():
    assert convert(4.54609, Unit.LITERS, Unit.UK_GALLONS) == pytest.approx(1)


def test_converts_ten_liters_to_us_gallons():
    assert convert(10, Unit.LITERS, Unit.US_GALLONS) == pytest.approx(2.641720524)


def test_converts_ten_liters_to_uk_gallons():
    assert convert(10, Unit.LITERS, Unit.UK_GALLONS) == pytest.approx(2.199692483)


def test_rejects_an_unsupported_conversion_pair():
    with pytest.raises(
        ValueError,
        match="Unsupported conversion: kilometers to fahrenheit",
    ):
        convert(1, Unit.KILOMETERS, Unit.FAHRENHEIT)
