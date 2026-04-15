# Calc Refactor — C# Walkthrough

This kata ships in **middle gear** — the whole C# implementation landed in one commit once the characterization set was understood. This walkthrough explains **why the refactored design came out the shape it did**, not how the commits unfolded.

This is an **F2 refactoring kata**: one primary entity (`Calculator`), one small test-folder builder (`CalculatorBuilder`), and a characterization set ([`../SCENARIOS.md`](../SCENARIOS.md)) that captures the legacy WPF calculator's behavior — *with its bugs fixed*. Read the repo [Gears section](../../README.md#gears--bridging-tdd-and-bdd) for why middle gear is the deliberate choice on a refactor-then-ship move.

## The Refactoring Move

The original kata points at a WPF calculator where every arithmetic rule lives inside a button's `Click` handler. The refactor is not *"unit-test the event handlers"* — it is *"pull the arithmetic out of the UI, commit to a pressable `Calculator` surface, characterize what the legacy actually did (bugs and all), decide which of those were intent and which were accidents, and ship a clean implementation whose tests pin the contract."*

In this reference the UI is already gone. Tests talk to `Calculator` directly:

```csharp
ACalculator().PressKeys("1+2=").Build().Display.Should().Be("3");
```

That single line would be four `Press()` calls without the builder. Across twenty scenarios the reduction matters.

## Scope — Calculator Core Only

WPF, `TextBox`, event handlers, memory buttons, decimal entry, and floating-point arithmetic are **out of scope**. See [`../README.md`](../README.md#scope--calculator-core-only) for the full list. The reference calculator is **integer-only** — a deliberate tightening that keeps the characterization set decidable without losing any of the legacy's teaching moments.

## The Design at a Glance

```
Calculator ──Press(key)──> (mutates internal state)
  Display: string             ──> "0" | "42" | "-6" | "Error"

Keys (constants)            DisplayStrings (constants)
  Equals = '='                Zero  = "0"
  Clear  = 'C'                Error = "Error"

ErrorMessages
  UnknownKey(key) = "unknown key: {key}"

CalculatorBuilder (tests/) ──Build──> Calculator
```

Three files under `src/CalcRefactor/` (the entity plus two constants classes) and one builder under `tests/CalcRefactor.Tests/`. That is the whole F2 surface.

## Why `Calculator` Is a Class, Not a Record

`Calculator` has **identity through time** — each press mutates state. Two calculators that were pressed the same sequence *might* currently match, but they are not *the same calculator*. Making it a `record` would buy nothing useful (value-equality over mutable internal state is a trap) and it would cost us the straightforward `Press(char key)` method that models exactly what the user does.

Contrast with [`password/`](../../password/) where `Policy` is a `record` — a policy is a pure value; a calculator is a thing with a running state. Different primitives, for reasons.

## Why the Display is a String (Not an `int`)

`"Error"` is not an integer. Neither is `"0"` with a distinguished leading-zero semantic. The display *is* a display — it is what the user would see in the WPF `TextBox`. Every scenario in [`../SCENARIOS.md`](../SCENARIOS.md) asserts on this exact string, and having `Display` return a `string` lets the tests read as what the user reads:

```csharp
calculator.Display.Should().Be("Error");
```

An `int?` with a separate `bool InError` would force every test to assert two things at once and would silently lose the `"0"` vs `"00"` distinction.

## Why Consecutive Operators Produce `"Error"` (The Legacy Bug Fix)

The original kata brief calls out `1++2 = 3` as wrong — the WPF implementation happily consumed the second `+` and did the addition anyway. There are two reasonable characterizations of what the *fix* should be:

1. Treat the second operator as a replacement for the first (so `1+*2` means `1*2`).
2. Reject the sequence as malformed — display `"Error"`.

This reference picks **(2)**. It matches the brief's explicit framing that `1++2 = 3` is *wrong* (the user slipped; the calculator should say so, not pretend), it is byte-identical across languages, and it keeps the error state a single concept: *the calculator is in an unrecoverable state until `'C'`.* Option (1) requires a subtler contract — "operator replaces pending operator, unless an operand has been entered" — and loses the cleanness.

Scenario 10 pins this: `"1++2="` produces `"Error"`. Not `"3"`. The commit history does not preserve the original bug; the characterization does.

## Why Division By Zero Also Produces `"Error"`

Same error state, same "sticky until `'C'`" semantics. The legacy WPF calculator almost certainly crashed or displayed `Infinity`; neither is useful. `"Error"` is the one spelling the spec commits to.

Division by zero and consecutive operators share the error state *deliberately* — from the user's perspective they are both "I did something nonsensical; the calculator says so; `C` to recover." Modeling them as two different error states would over-specify.

## Why `Apply` Uses a `switch` Statement

This is the one place C# pattern-matching is the right tool. Four operators, four branches, one result per branch — plus the `/` case that short-circuits into `EnterError()`. A dictionary of `char -> Func<int,int,int>` would be marginally more elegant for a larger operator set and wrong for four cases: it loses the natural place to put the divide-by-zero check.

The walkthrough notes this explicitly because [the plan's F2 style conventions](../../docs/plans/2026-04-14-remaining-katas.md) prefer named constants and typed dispatch — but the operator itself is already a `char` and the branches are the rules. Extracting further hurts rather than helps.

## Why `CalculatorBuilder` Takes a Key String, Not Individual Methods

Compare two styles:

```csharp
// Method-per-key (rejected):
ACalculator().Digit(1).Plus().Digit(2).Equals().Build()

// Key-string (shipped):
ACalculator().PressKeys("1+2=").Build()
```

The shipped form reads as *the key sequence the user pressed*. The method-per-key form reads as code dressed up as a DSL — and it would be four times longer for every scenario. The calculator's interface is already a single-character keyset; the builder simply lets a test write the sequence inline.

Nineteen lines of builder including braces. That is under the F2 budget and the builder spends the lines well — no flags, no parallel methods, no object-mother, no tuple return.

## What Is Deliberately Not Modeled

- **No UI.** No WPF, no `TextBox`, no event handlers.
- **No floating-point arithmetic.** Integer operands, integer results, integer division truncating toward zero (matching C# `/`).
- **No operator precedence.** `"2+3*4="` is `20`, not `14`. The legacy calculator had none; this characterizes that.
- **No memory buttons, no percentage, no square root.** None of them were in the original brief.

Each of those is a deliberate stretch. See the top-level [README](../README.md#what-is-not-modeled) for the full list.

## Scenario Map

The twenty scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/CalcRefactor.Tests/CalculatorTests.cs`, one `[Fact]` per scenario, with test names matching the scenario titles verbatim (modulo C# underscore convention).

## How to Run

```bash
cd calc-refactor/csharp
dotnet test
```

Expected: **20 passed, 0 failed.**
