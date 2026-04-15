# Time Zone Converter — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Domain Rules

- Inputs: a **naive local date-time** (year, month, day, hour, minute, second), a **source offset** from UTC, and a **target offset** from UTC. Offsets are signed durations (hours + minutes), e.g. `-05:00`, `+09:00`, `+05:30`, `+00:00`.
- Output: a naive local date-time that represents the same UTC instant rendered in the target offset.
- Algorithm: subtract the source offset from the local time to get UTC, then add the target offset to get the target-local time. Carry across minute / hour / day / month / year boundaries using the standard library's date arithmetic.
- Scope is **fixed UTC offsets only**. IANA zone names, daylight saving transitions, and ambiguous/skipped local times are out of scope for this kata — the spec's hint names fixed offsets as the starting point and this reference stays there.

## Test Scenarios

1. **Identity** — `2024-06-15 12:00:00` from `+00:00` to `+00:00` stays `2024-06-15 12:00:00` (no offset change, no conversion)
2. **Westward** — `2024-06-15 12:00:00` UTC to `-05:00` (EST) becomes `2024-06-15 07:00:00` (subtract five hours, same day)
3. **Eastward** — `2024-06-15 12:00:00` UTC to `+09:00` (JST) becomes `2024-06-15 21:00:00` (add nine hours, same day)
4. **Cross-zone without UTC** — `2024-06-15 09:00:00` at `-05:00` to `+09:00` becomes `2024-06-15 23:00:00` (source is 14:00 UTC; target adds nine hours)
5. **Forward across midnight** — `2024-06-15 22:00:00` UTC to `+05:00` becomes `2024-06-16 03:00:00` (rolls into next day)
6. **Backward across midnight** — `2024-06-15 02:00:00` UTC to `-05:00` becomes `2024-06-14 21:00:00` (rolls into previous day)
7. **Half-hour offset** — `2024-06-15 12:00:00` UTC to `+05:30` (IST) becomes `2024-06-15 17:30:00` (non-integer-hour offset)
8. **Quarter-hour offset** — `2024-06-15 12:00:00` UTC to `+05:45` (NPT) becomes `2024-06-15 17:45:00` (forty-five-minute offset)
9. **Forward across month boundary** — `2024-06-30 23:30:00` UTC to `+02:00` becomes `2024-07-01 01:30:00` (June → July)
10. **Backward across year boundary** — `2024-01-01 01:00:00` UTC to `-05:00` becomes `2023-12-31 20:00:00` (previous year)
11. **Forward across leap-day boundary** — `2024-02-29 23:00:00` UTC to `+02:00` becomes `2024-03-01 01:00:00` (leap day → March 1)
12. **International date line** — `2024-06-15 10:00:00` at `+12:00` to `-12:00` becomes `2024-06-14 10:00:00` (twenty-four-hour swing, previous day at same clock time)
