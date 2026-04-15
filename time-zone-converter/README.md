# Time Zone Converter

Given a local time and a fixed UTC offset for both the source and target zones, return the same instant rendered in the target zone's local time.

This kata ships in **Agent Full-Bake** mode at the F1 tier: an algorithmic kata with **no builders**. The inputs are a naive local date-time plus two fixed UTC offsets (e.g. `-05:00`, `+09:00`, `+05:30`); the output is a naive local date-time in the target zone. The teaching scope is deliberately narrowed to **fixed offsets** — no IANA zones, no DST rules, no ambiguous-local-time handling around transitions. The kata spec's hint opens with "Start by implementing basic time zone conversions using fixed offsets, then add support for daylight saving time"; this reference stays in that first step, where the arithmetic is pure: normalise to UTC, add the destination offset, carry across date boundaries.

The single primitive is `convert(local, fromOffset, toOffset)`; the per-field "year, month, day, hour, minute" list from a full timezone library reduces to this one function over the standard library's date-time value. See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
