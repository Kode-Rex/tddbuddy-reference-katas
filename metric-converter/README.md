# Metric Converter

Convert a numeric value from a metric unit to an imperial unit across four dimensions: **length**, **temperature**, **weight**, and **volume**. Each dimension has its own conversion; volume distinguishes between US and UK gallons, so the target-side variant is part of the input.

This kata ships in **Agent Full-Bake** mode at the F1 tier: an algorithmic kata with **no builders**. The function surface is four pure conversions sharing a single entry point — `convert(value, Unit.From, Unit.To) -> number`. The domain has one small closed set — the five units the converter understands (`Kilometers`, `Celsius`, `Kilograms`, `Liters`, and the target side's `Miles`, `Fahrenheit`, `Pounds`, `UsGallons`, `UkGallons`) — so modelling it as a typed enum/union prevents `"kg"` vs `"Kilograms"` drift at call sites.

**Typed units, not strings.** The closed set is the ubiquitous language. C# uses `enum Unit`; TypeScript uses a string-literal union; Python uses `StrEnum`. Illegal pairs (e.g. `Kilometers → Fahrenheit`) raise a domain exception — the converter is not a calculator.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
