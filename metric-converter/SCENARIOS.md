# Metric Converter — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Unit** | One of `Kilometers`, `Miles`, `Celsius`, `Fahrenheit`, `Kilograms`, `Pounds`, `Liters`, `UsGallons`, `UkGallons` |
| **convert** | The pure function that takes a value and a `from`/`to` unit pair and returns the converted value |

## Domain Rules

- **Kilometers → Miles:** `miles = kilometers * 0.621371`
- **Celsius → Fahrenheit:** `fahrenheit = (celsius * 1.8) + 32`
- **Kilograms → Pounds:** `pounds = kilograms / 0.45359237`
- **Liters → US Gallons:** `usGallons = liters / 3.785411784`
- **Liters → UK Gallons:** `ukGallons = liters / 4.54609`
- Any unsupported `(from, to)` pair raises a domain error — the converter is one-directional metric→imperial.

## Test Scenarios

1. **converts kilometers to miles** — `convert(1, Kilometers, Miles) ≈ 0.621371`
2. **converts kilometers to miles for a larger value** — `convert(10, Kilometers, Miles) ≈ 6.21371`
3. **converts zero kilometers to zero miles** — `convert(0, Kilometers, Miles) == 0`
4. **converts celsius to fahrenheit for freezing point** — `convert(0, Celsius, Fahrenheit) == 32`
5. **converts celsius to fahrenheit for body temperature** — `convert(30, Celsius, Fahrenheit) == 86`
6. **converts negative celsius to fahrenheit** — `convert(-40, Celsius, Fahrenheit) == -40`
7. **converts kilograms to pounds** — `convert(5, Kilograms, Pounds) ≈ 11.0231131`
8. **converts zero kilograms to zero pounds** — `convert(0, Kilograms, Pounds) == 0`
9. **converts liters to US gallons** — `convert(3.785411784, Liters, UsGallons) == 1`
10. **converts liters to UK gallons** — `convert(4.54609, Liters, UkGallons) == 1`
11. **converts ten liters to US gallons** — `convert(10, Liters, UsGallons) ≈ 2.641720524`
12. **converts ten liters to UK gallons** — `convert(10, Liters, UkGallons) ≈ 2.199692483`
13. **rejects an unsupported conversion pair** — `convert(1, Kilometers, Fahrenheit)` raises a domain error
