# Calc Refactor

A calculator whose legacy incarnation lived entirely in button-click event handlers: no tests, no domain model, and a handful of quiet bugs — including the infamous `1++2 = 3` that the original kata brief calls out by name. The work is two moves, in order: **characterize the legacy behavior with tests, then refactor the implementation until those tests still pass against code you would be happy to hand to a teammate.**

This kata ships in **Agent Full-Bake** mode at the **F2 tier**. One primary entity, `Calculator`, receives button presses and exposes a `Display` string; a small test-folder `CalculatorBuilder` makes each scenario read as the key sequence the user pressed, not the scalar assembly of the calculator's internal state.

## The Refactoring Move

A refactoring kata is not *"write tests for what exists"* — it is *"decide what the legacy actually promises, write that down as characterization tests, then rewrite the implementation cleanly."* In the shipped reference:

- [`SCENARIOS.md`](SCENARIOS.md) is the characterization set. Every entry captures a behavior the legacy code exhibits (or *should* exhibit — see the bug fixes below) and commits the three language ports to it byte-for-byte.
- Each language ships a **clean** `Calculator` implementation. The tests do not exercise WPF event handlers — they exercise a pressable `Calculator` that accepts buttons and returns what the display would show. The refactoring is *complete* in the reference; the characterization work is what earns the right to refactor.
- **Bug fixes are part of the spec.** The original brief explicitly calls out `1++2 = 3` as wrong. The characterization set codifies the fixed behavior: pressing two operators in a row sets the display to `"Error"`. Every language must agree. The legacy's accidental arithmetic is not preserved.

This is the pattern for every refactoring kata in the set: SCENARIOS.md is the contract between the legacy behavior (cleaned up) and the new implementation.

## Scope — Calculator Core Only

The original TDD Buddy spec references a full WPF application: buttons, event handlers, `TextBox`, and UI wiring. **All UI is out of scope** for this reference. The calculator here is a pure domain object with a `Press(key)` method and a `Display` property. That is exactly enough to characterize the original's arithmetic, sequencing, and error behavior without dragging WPF or any UI framework into the tests.

### What Is In Scope

- Digit entry (`0`–`9`), including multi-digit operand assembly.
- The four binary operators: `+`, `-`, `*`, `/`.
- `=` (equals) — evaluates the pending operation; repeated `=` reuses the last operator and operand.
- `C` (clear) — resets the calculator to its initial state (display `"0"`).
- Error states for division by zero and consecutive operator presses. Error is sticky until `C`.

### What Is Not Modeled

- WPF / any UI layer. Tests talk to the `Calculator` directly.
- Floating-point arithmetic, decimal point entry, scientific notation. The reference calculator is **integer-only** — matching the WPF original's apparent intent and keeping the characterization set decidable.
- Memory buttons (`M+`, `MR`, etc.), percentage, square root, or any other extended-calculator features. None of them appear in the original kata brief.

See [`SCENARIOS.md`](SCENARIOS.md) for the complete characterization set.

## Why This Is F2

- **One primary entity** — `Calculator`. No collaborators, no injected clocks, no repositories.
- **Light builder** — `CalculatorBuilder` composes a key sequence into one readable line per scenario. Without it, tests would spell out `calculator.Press('1'); calculator.Press('+'); calculator.Press('2'); calculator.Press('=');` four times per scenario; with it, the setup reads `aCalculator().PressKeys("1+2=").Build()`. That shift is the whole F2 reason for existing.
- **Rich return type, not bool.** The `Display` is a string — `"0"`, `"42"`, `"-7"`, `"Error"`. Tests assert on the exact display string, which *is* the spec.

## How to Read This Kata

1. Read this README for scope and the refactoring-move framing.
2. Read [`SCENARIOS.md`](SCENARIOS.md) for the characterization set — the contract between legacy intent and clean implementation.
3. Pick a language and read its `WALKTHROUGH.md` for design rationale.
4. Run the tests.

Reference walkthroughs: [`csharp/`](csharp/WALKTHROUGH.md), [`typescript/`](typescript/WALKTHROUGH.md), [`python/`](python/WALKTHROUGH.md).
