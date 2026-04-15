# Fizz Buzz Whiz — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Domain Rules

- For any integer `n`:
  - If `n` is divisible by **both 3 and 5** (i.e. divisible by 15) → `"FizzBuzz"`
  - Else if `n` is divisible by **3** → `"Fizz"`
  - Else if `n` is divisible by **5** → `"Buzz"`
  - Otherwise → `n` formatted as a decimal string

The "Whiz" prime bonus from the original TDD Buddy prompt is **not** implemented here — see the top-level [`README.md`](README.md).

## Test Scenarios

1. **1 returns "1"** — not divisible by 3 or 5
2. **2 returns "2"** — not divisible by 3 or 5
3. **3 returns "Fizz"** — divisible by 3
4. **5 returns "Buzz"** — divisible by 5
5. **6 returns "Fizz"** — another multiple of 3
6. **10 returns "Buzz"** — another multiple of 5
7. **15 returns "FizzBuzz"** — divisible by both 3 and 5
8. **30 returns "FizzBuzz"** — another multiple of 15
