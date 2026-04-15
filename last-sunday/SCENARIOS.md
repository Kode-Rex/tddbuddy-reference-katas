# Last Sunday — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Domain Rules

- Inputs: `year` (four-digit Gregorian year) and `month` (1-indexed: `1` = January, `12` = December).
- Output: the date of the last Sunday of that month.
- Algorithm: take the last day of the month, then walk back zero-to-six days until the weekday is Sunday.
- Month 1-indexing is the human convention; implementations that use a 0-indexed month internally (e.g. JavaScript `Date`) convert at the boundary.

## Test Scenarios

All twelve months of 2013 appear in the TDD Buddy prompt's example table; the scenarios below sample that table plus leap-year edge cases.

1. **January 2013** — last Sunday is `2013-01-27` (month with 31 days; last day is Thursday)
2. **February 2013** — last Sunday is `2013-02-24` (non-leap February; 28 days)
3. **March 2013** — last Sunday is `2013-03-31` (31 days; last day itself is Sunday — zero-step walk-back)
4. **April 2013** — last Sunday is `2013-04-28` (30-day month)
5. **June 2013** — last Sunday is `2013-06-30` (30-day month; last day is Sunday)
6. **December 2013** — last Sunday is `2013-12-29` (year-end boundary)
7. **February 2020** — last Sunday is `2020-02-23` (leap February; 29 days, last day is Saturday)
8. **February 2032** — last Sunday is `2032-02-29` (leap February; last day *is* Sunday)
9. **February 2100** — last Sunday is `2100-02-28` (century non-leap; 28-day February)
10. **December 2000** — last Sunday is `2000-12-31` (four-hundred-divisible leap year; last day of year is Sunday)
