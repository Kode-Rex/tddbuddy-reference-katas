# Leap Year — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Domain Rules

- For any integer `year`:
  - If `year` is divisible by **400** → leap year (`true`)
  - Else if `year` is divisible by **100** → not a leap year (`false`)
  - Else if `year` is divisible by **4** → leap year (`true`)
  - Otherwise → not a leap year (`false`)

Input validation (zero, negative years, non-integers) from the original TDD Buddy prompt is **not** implemented here — see the top-level [`README.md`](README.md).

## Test Scenarios

1. **2023 is not a leap year** — not divisible by 4
2. **2024 is a leap year** — divisible by 4, not by 100
3. **2020 is a leap year** — another typical leap year divisible by 4
4. **1900 is not a leap year** — divisible by 100 but not by 400
5. **2100 is not a leap year** — another century year not divisible by 400
6. **2000 is a leap year** — divisible by 400
7. **1600 is a leap year** — another century divisible by 400
8. **2001 is not a leap year** — odd year, not divisible by 4
