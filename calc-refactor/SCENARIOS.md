# Calc Refactor — Scenarios

Shared characterization specification satisfied by the C#, TypeScript, and Python implementations.

## Scope

This specification covers the **pure calculator core** — button presses in, display string out. The WPF UI, event handlers, and any memory/percentage/decimal features are **out of scope**. See the top-level [`README.md`](README.md#scope--calculator-core-only) for the full out-of-scope list.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Calculator** | The primary entity. Accepts button presses via `press(key)` and exposes the current `display` string. The display is what the legacy WPF application would show in its `TextBox` at any point in a key sequence. |
| **Key** | A single character the user pressed: `'0'`–`'9'` for digits, `'+' '-' '*' '/'` for operators, `'='` for evaluate, `'C'` for clear. Any other character raises a language-idiomatic argument error — it is not a defined key. |
| **Display** | The string shown to the user. `"0"` in the initial state; a decimal integer otherwise (no leading zeros, a leading `-` for negatives); `"Error"` in an error state. |
| **Pending operator** | The most recent binary operator pressed, remembered until the next operand arrives or evaluation runs. |
| **Error state** | A sticky state entered by division by zero or by pressing two operators in a row. The display reads `"Error"`; every subsequent key is ignored *except* `'C'`, which returns the calculator to its initial state. |
| **CalculatorBuilder** | Test-folder fluent builder. `aCalculator().pressKeys("1+2=").build()` creates a calculator and drives the key sequence; `.build()` returns the calculator in its final state so tests can read `calculator.display`. Exists so every test opens with one line naming the sequence it cares about. |

## Domain Rules

- **Initial state.** A fresh calculator shows `"0"`. No pending operator; no stored accumulator.
- **Digit entry.** Pressing a digit appends it to the current operand. The operand's leading `"0"` is replaced, not appended to — `press('0'); press('5')` leaves the display at `"5"`, not `"05"`.
- **Operator entry after an operand.** Pressing `+ - * /` stores the current display as the left-hand side and remembers the operator; the next digit starts a fresh right-hand operand.
- **Two operators in a row ⇒ error.** The legacy bug `1++2 = 3` is characterized as fixed: pressing two operators consecutively transitions to the error state. Display becomes `"Error"`.
- **Equals.** Pressing `=` evaluates `left operator right` and replaces the display with the result. The pending operator and right operand are remembered for repeated `=` — pressing `=` again applies the same operator to the current result and the remembered right operand (classic desk-calculator behavior: `2 + 3 = =` shows `"8"`).
- **Equals with no pending operator.** A no-op — the display stays as-is.
- **Division by zero ⇒ error.** `n / 0 =` transitions to the error state.
- **Integer arithmetic.** All operands and results are integers. Division truncates toward zero: `7 / 2 = 3`, `-7 / 2 = -3`.
- **Clear.** `'C'` resets the calculator to the initial state, even from the error state.
- **Error is sticky.** Once in error, every key other than `'C'` is ignored; the display keeps reading `"Error"`.

## Test Scenarios

1. **A fresh calculator displays zero** — `aCalculator().build().display` is `"0"`.
2. **Pressing a single digit displays that digit** — pressing `"7"` shows `"7"`.
3. **Pressing multiple digits builds a multi-digit operand** — pressing `"123"` shows `"123"`.
4. **Leading zero is replaced by the first non-zero digit** — pressing `"05"` shows `"5"`, not `"05"`.
5. **Addition of two operands** — pressing `"1+2="` shows `"3"`.
6. **Subtraction of two operands** — pressing `"9-4="` shows `"5"`.
7. **Multiplication of two operands** — pressing `"6*7="` shows `"42"`.
8. **Integer division truncates toward zero** — pressing `"7/2="` shows `"3"`.
9. **Division by zero enters the error state** — pressing `"5/0="` shows `"Error"`.
10. **Consecutive operators enter the error state (legacy bug fix)** — pressing `"1++2="` shows `"Error"` (*not* `"3"`).
11. **Error is sticky — further keys are ignored** — pressing `"5/0=123+4="` shows `"Error"`.
12. **Clear resets from the error state** — pressing `"5/0=C"` shows `"0"`.
13. **Clear resets from a normal state** — pressing `"42C"` shows `"0"`.
14. **Equals with no pending operator is a no-op** — pressing `"42="` shows `"42"`.
15. **Repeated equals reapplies the last operator and operand** — pressing `"2+3=="` shows `"8"` (second `=` adds the remembered `3` again).
16. **Chained operators evaluate left-to-right** — pressing `"2+3*4="` shows `"20"` (`(2+3)*4`, not `2+(3*4)`; the calculator has no operator precedence, matching the legacy).
17. **Operator after equals continues from the result** — pressing `"2+3=*4="` shows `"20"`.
18. **A new digit after equals starts a fresh calculation** — pressing `"2+3=7"` shows `"7"`, and `"2+3=7+1="` shows `"8"`.
19. **Negative results display with a leading minus** — pressing `"3-9="` shows `"-6"`.
20. **An unknown key raises an argument error** — pressing `'x'` raises `ArgumentException` / `Error` / `ValueError` (language-idiomatic). The message is identical across languages: `"unknown key: x"`.
