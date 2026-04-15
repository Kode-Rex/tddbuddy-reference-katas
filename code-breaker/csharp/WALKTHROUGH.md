# Code Breaker — C# Walkthrough

This kata ships in **middle gear** — the whole C# implementation landed in one commit once the design was understood. This walkthrough explains **why the design came out the shape it did**, not how the commits unfolded.

It is the F2 "light builder" reference for a two-entity kata: `Secret` and `Guess` both need readable four-peg literals in tests, so both get a small test-folder builder. The scoring engine itself (`Feedback.Compute`) is a pure function over the two codes.

## Scope — Feedback Engine Only

Random secret generation, attempt tracking, and the full game loop are **out of scope**. See the kata [`README.md`](../README.md#stretch-goals-not-implemented-here) for the stretch-goal list.

## The Design at a Glance

```
Peg (enum One..Six)
Secret ──ScoreAgainst(Guess)──> Feedback (record)
Guess                            ExactMatches: int
                                 ColorMatches: int
                                 IsWon: bool (derived)
                                 Render(): "++--"

SecretBuilder (tests/) ──Build──> Secret
GuessBuilder  (tests/) ──Build──> Guess
```

Six files under `src/CodeBreaker/` (the enum, the code-length constant, the two code types, the feedback record) and two small builders under `tests/`.

## Why `Peg` Is an Enum

The spec says "digits 1–6". Modeling them as an `enum Peg { One=1 … Six=6 }` closes off every other value at compile time: you cannot accidentally construct a peg with the digit 7 or with the character `'1'`. Tests read `One, Two, Three, Four` — closer to prose than a raw array of ints — and `using static CodeBreaker.Peg;` lets the builder calls omit the `Peg.` prefix without ambiguity.

A raw `int` would have been simpler to write; it would also have forced a range-check inside `Secret` / `Guess` constructors, and tests would have had to remember the 1–6 contract instead of reading it off the type.

## Why `Secret` and `Guess` Are Distinct Types

They have the same shape: four pegs. They are not the same *role*: one is the target, the other the attempt. Making them distinct types means `Secret.ScoreAgainst(Guess)` cannot be miscalled with arguments swapped — the compiler enforces which one you hold. Two tiny classes, each with a three-line constructor that validates length, do this work.

Length validation (`pegs.Count != CodeLength.Pegs`) lives in the constructors so any `Secret`/`Guess` value at rest is well-formed. `CodeLength.Pegs = 4` is named rather than inline because (a) F2 is Full-Bake mode and named constants win, and (b) the "4" appears in three places (both constructors and the `IsWon` derivation); centralizing it would be the minimum fix if this kata later grew a "configurable code length" stretch goal.

## Why `Feedback` Is a Record with Derived `IsWon` and `Render()`

`Feedback` has no identity — two feedbacks reporting `(2 exact, 2 color)` *are* the same feedback. Value equality for free via `record`, along with a useful `ToString()` for test failure diagnostics.

`IsWon` is derived (`ExactMatches == CodeLength.Pegs`). Storing it would invite drift; deriving it means there is one source of truth for "the player has broken the code."

`Render()` produces the canonical `"+"…"-"…` string used in the kata spec's test table. Keeping both the structured counts **and** the string on `Feedback` means tests can assert on whichever is more readable per scenario — `feedback.ExactMatches.Should().Be(2)` when the count is what matters, `feedback.Render().Should().Be("++--")` when the spec literally names a string. Both views stay consistent because both come from the same two ints.

## The Duplicate-Peg Algorithm — Two Passes

The "tricky case" in the spec (`1124` vs `5167`) is the whole reason duplicate handling matters. The implementation uses **two passes**:

1. **Exact-match pass.** For each position, if the pegs agree, increment `ExactMatches` and mark the position as consumed on *both* sides by *not* copying it into the unmatched lists.
2. **Color-match pass.** For each peg in the unmatched-guess list, try to find it in the unmatched-secret list; if found, remove it and increment `ColorMatches`. This consumes each unmatched secret peg at most once.

A one-pass implementation that counts value occurrences (`min(secretCount[v], guessCount[v])`) can also work — but only if you subtract the exact matches first. The two-pass version is chosen here because it reads straight off the domain rule: "positions consumed by an exact match are not eligible for a color-only match," then "each unmatched secret peg can pair with at most one unmatched guess peg."

## Why the Builders Exist — The F2 Signature Pattern

Eleven scenarios need eleven four-peg codes. Without a builder, every test opens with:

```csharp
var secret = new Secret(new[] { Peg.One, Peg.Two, Peg.Three, Peg.Four });
```

That repeats twice per scenario (once for the secret, once for the guess) and buries the variation — "which four pegs?" — inside a raw array literal. With `SecretBuilder` and `GuessBuilder`:

```csharp
var secret = new SecretBuilder().With(One, Two, Three, Four).Build();
var guess  = new GuessBuilder().With(One, Two, Four, Three).Build();
```

One line each, the four pegs sit in a positional argument list that reads left-to-right as the code, and `using static CodeBreaker.Peg;` lets the peg names themselves carry the meaning. Each builder is twelve lines — comfortably inside the F2 budget.

## What Is Deliberately Not Modeled

- **No `SecretGenerator`** — there is no random-code generation here, so there is no collaborator to fake.
- **No `Game` aggregate** — there is no attempts counter, no history of guesses, no loss condition.
- **No configurable code length or peg range** — both are stretch goals; see the repo README.
- **No domain-specific exceptions beyond `ArgumentException`** — feedback is a return value, never a throw. The only throws are the constructor length checks, and those are boundary guards, not domain events.

Every omission above points at an F3 extension.

## Scenario Map

The twelve scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/CodeBreaker.Tests/FeedbackTests.cs`, one `[Fact]` per scenario. The peg values in scenario 1 are substituted from the spec's `5678` into `5566` to respect the 1–6 peg range of the kata; the feedback behavior is identical.

## How to Run

```bash
cd code-breaker/csharp
dotnet test
```
