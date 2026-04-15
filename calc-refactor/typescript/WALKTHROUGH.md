# Calc Refactor — TypeScript Walkthrough

Same design as the [C# reference](../csharp/WALKTHROUGH.md). This walkthrough is a **delta** — it names what is idiomatic to TypeScript rather than re-arguing the design.

## Scope — Calculator Core Only

WPF UI, memory buttons, decimals, floating-point arithmetic are **out of scope**. See [`../README.md`](../README.md#scope--calculator-core-only) for the full list.

## The TypeScript Shape

- **`Calculator` is a class with private fields and a `get display()` accessor.** It has identity through time (each `press()` mutates state), which is exactly what a class is for. `readonly`-everything would not fit — the calculator *is* mutable by design.
- **Keys, display strings, and the error-message builder colocate in `src/calculator.ts`** under three `as const` objects. F2 style convention: TypeScript idiom is to colocate small related types in one module rather than scattering them across `Keys.ts` / `DisplayStrings.ts` / `ErrorMessages.ts`. The rule-name strings stay byte-identical to the C# `Keys`/`DisplayStrings`/`ErrorMessages` values and the Python `KEYS`/`DISPLAY`/`ERRORS` values.
- **`Operator` is a union type** (`'+' | '-' | '*' | '/'`), and `isOperator(c): c is Operator` is a type predicate — the compiler narrows the switch arms in `apply()` to exhaustive cases with no default branch. This is what TypeScript's discriminated-union narrowing is *for*; reaching for it keeps the operator table as lean as the C# switch.
- **Integer division uses `Math.trunc(a / b)`.** JavaScript's `/` is floating-point; truncating toward zero keeps `"7/2="` and `"-7/2="` byte-identical across languages. Character-class-loop parity is not the point here — the arithmetic parity is.
- **`Error` is the exception type** where C# throws `ArgumentException` and Python raises `ValueError`. The message string (`"unknown key: x"`) is identical across languages; the exception type is not.

## Why `CalculatorBuilder` Lives in `tests/`

Same F2 rationale as C#: every scenario needs a different key sequence, and the builder lets each test open with one line naming the sequence. `aCalculator().pressKeys('1+2=')` is an inline factory + fluent-setter idiom that reads as the user's hand on the calculator. Twenty-one lines including braces — within the 10–30 line F2 budget.

## Scenario Map

The twenty scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/calculator.test.ts`, one `it()` per scenario, with titles matching the scenario statements.

## How to Run

```bash
cd calc-refactor/typescript
npm install
npx vitest run
```

Expected: **20 tests passed.**
