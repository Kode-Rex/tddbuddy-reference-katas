# Age Calculator — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Domain Rules

- Inputs: `birthdate` and `today`, both standard-library date values (no time-of-day component).
- Output: the person's age in whole years at the reference date.
- Algorithm: take the difference in years, then subtract one if the birthday has not yet occurred in the reference year (i.e. `(today.month, today.day) < (birthdate.month, birthdate.day)`).
- **Birthday-as-boundary:** on the exact birthday, the person *has* turned that age. `2023-10-28` for a `2016-10-28` birthdate is age `7`, not `6`.
- **Leap-day birthdays in non-leap years:** a person born on `2000-02-29` turns one year older on `2001-03-01`, not `2001-02-28`. The (month, day) comparison handles this without special-casing: `(2, 28) < (2, 29)` is `true`, so age increments only once March arrives.
- **Future birthdate:** if `birthdate > today`, the inputs are nonsensical for age calculation; the implementation throws an idiomatic exception with the byte-identical message `birthdate is after today`.
- **Same-day birth:** if `birthdate == today`, the person is age `0` (born today).

## Test Scenarios

1. **Zenith was born on 28 October 2016; on 5 November 2022 she is 6.** (The TDD Buddy prompt's canonical example; birthday already passed in the reference year.)
2. **Zenith on her seventh birthday, 28 October 2023, is 7.** (Birthday-as-boundary: exact-day turn-over.)
3. **Zenith on 27 October 2023, the day before her seventh birthday, is 6.** (Birthday-not-yet-reached in the reference year.)
4. **A person born 1 January 2000 on 31 December 2024 is 24.** (Year-end boundary; birthday already passed way back in January of the reference year.)
5. **A person born 1 January 2000 on 1 January 2000 is 0.** (Same-day-as-today: born today, age zero.)
6. **A leap-day baby born 29 February 2000 on 28 February 2001 is 0.** (Leap-day birthday in a non-leap year has not yet occurred on Feb 28.)
7. **A leap-day baby born 29 February 2000 on 1 March 2001 is 1.** (Leap-day birthday in a non-leap year rolls over on March 1.)
8. **A leap-day baby born 29 February 2000 on 29 February 2004 is 4.** (Actual leap-day birthday in a leap year: exact-day turn-over.)
9. **A person born 31 December 1999 on 1 January 2000 is 0.** (Born yesterday, across a year boundary; age still zero because 365 days have not elapsed.)
10. **A person born 15 June 1990 on 14 June 2024 is 33.** (Day-before-birthday in the reference year; `(6, 14) < (6, 15)`.)
11. **Throws when birthdate is after today.** Message: `birthdate is after today`. (Future-date guard — exception type is language-idiomatic: `ArgumentException` / `Error` / `ValueError`.)
