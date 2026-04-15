from enum import StrEnum


class Unit(StrEnum):
    KILOMETERS = "kilometers"
    MILES = "miles"
    CELSIUS = "celsius"
    FAHRENHEIT = "fahrenheit"
    KILOGRAMS = "kilograms"
    POUNDS = "pounds"
    LITERS = "liters"
    US_GALLONS = "us_gallons"
    UK_GALLONS = "uk_gallons"


def convert(value: float, from_unit: Unit, to_unit: Unit) -> float:
    match (from_unit, to_unit):
        case (Unit.KILOMETERS, Unit.MILES):
            return value * 0.621371
        case (Unit.CELSIUS, Unit.FAHRENHEIT):
            return (value * 1.8) + 32
        case (Unit.KILOGRAMS, Unit.POUNDS):
            return value / 0.45359237
        case (Unit.LITERS, Unit.US_GALLONS):
            return value / 3.785411784
        case (Unit.LITERS, Unit.UK_GALLONS):
            return value / 4.54609
        case _:
            raise ValueError(
                f"Unsupported conversion: {from_unit} to {to_unit}"
            )
