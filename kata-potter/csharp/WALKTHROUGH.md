# Kata Potter — C# Walkthrough

This kata ships in **middle gear** — the C# implementation landed in one commit once the design was understood. This walkthrough explains **why the design came out the shape it did**, not how the commits unfolded.

It is an **F2** reference: one primary entity (`Basket`), one small test-folder builder (`BasketBuilder`), a static `PricingRules` module holding the base price and the discount table, and one domain-specific exception for malformed book ids.

## Scope — Pure Domain Only

No catalogue service, no receipts with grouping breakdowns, no arbitrary series size, no currency localisation. See [`../README.md`](../README.md#stretch-goals-not-implemented-here) for the stretch-goal list. The five-title / fixed-discount structure is the spec.

## The Design at a Glance

```
PricingRules (static)
  ├── BasePrice = 8.00m
  ├── MinBookId, MaxBookId          — named range bounds
  ├── SetDiscount[]                 — discount by set size, indexed 1..5
  └── PriceOfSet(k)                 — k * BasePrice * (1 - SetDiscount[k])

BookOutOfRangeException             — builder-side validation

Basket
  └── Price() : decimal             — optimal-grouping price in Euros

BasketBuilder (tests/)
  ├── WithBook(series, count)       — validates series ∈ [1,5]
  └── Build() : Basket
```

Three source files under `src/KataPotter/` (Basket, PricingRules + BasketMessages grouped, BookOutOfRangeException) and one builder plus one test class under `tests/KataPotter.Tests/`.

## Why `decimal` and Not `double`

The spec is a money calculation with two-place decimals — €8.00, €15.20, €25.60. Every price the discount table can produce is representable exactly in decimal (the multipliers are 1.00, 0.95, 0.90, 0.80, 0.75, and the base is 8.00). `double` would introduce floating-point drift that leaks into test assertions; `decimal` matches the domain and lets tests compare with `Should().Be(51.20m)` rather than "within tolerance". The TypeScript and Python implementations pay a small tolerance cost because those languages don't have a first-class decimal type in the same idiom; C# doesn't need to.

## Why `PricingRules` Is a Static Module

The rules are **fixed** by the kata spec — five titles, five tiers, this discount table. Named constants live together in `PricingRules` rather than being scattered into `Basket`, because the rules are what the reader actually asks about ("what are the discounts?") and they are the part a stretch-goal refactor would lift into a configurable `PricingRules` collaborator. Keeping them one file with one name makes the seam visible without actually drawing it.

F2's rule is **named constants for business numbers**. `BasePrice`, `MaxBookId`, and the `SetDiscount` array are all named. The discount *values* (`0.05m`, `0.10m`, `0.20m`, `0.25m`) are inlined in the table literal — naming each would be noise because the reader reads them together, in order, and the pattern is the signal.

## Why the Algorithm Is Greedy-Then-Adjust

The kata's canonical trap is the `[2, 2, 2, 1, 1]` basket: pure greedy produces a 5-set plus a 3-set (€51.60), but two 4-sets is cheaper (€51.20). A fully general solver (DP or enumeration over partitions) is overkill because the five-title / standard-discount structure admits a **single local swap** that captures every improvement:

> A 5-set plus a 3-set always costs more than two 4-sets.
>
> `5 * 8 * 0.75 + 3 * 8 * 0.90 = 30.00 + 21.60 = 51.60`
> vs.
> `2 * (4 * 8 * 0.80)           = 25.60 * 2    = 51.20`
>
> Savings per pairing: €0.40. No other swap between adjacent set sizes improves (a 5+2 → 4+3 swap is a wash — `30.00 + 15.20 = 45.20` vs `25.60 + 21.60 = 47.20` is *worse*, so keep greedy for that case; a 5+1 swap has nowhere to go; 4+2 → 3+3 is also worse).

So the implementation is: **greedy histogram + one swap pass**.

```csharp
var sets = GroupIntoSets(_counts);            // histogram: sets[k] = # of k-sized sets
AdjustFivePlusThreeIntoTwoFours(sets);        // swap (5,3) -> (4,4,4,4)... well, (4,4) per swap
return sum over k of sets[k] * PriceOfSet(k);
```

The histogram representation (rather than a list of `Set` objects) matters: the swap becomes three integer operations, and the total price is a dot product between `sets` and `PriceOfSet`.

## Why the Swap Is Proven Correct

`min(sets[5], sets[3])` is the *maximum* number of (5,3) pairs present. Replacing each pair with two 4-sets strictly reduces total price by €0.40. After the pass, no 5+3 pair remains. Every other local swap is either neutral or worsening, so no further adjustments are needed and the result is provably optimal for the five-title / standard-discount case.

If the discount table were configurable (the F3-shaped stretch goal), this argument would need to be rechecked; a general solver would then be a small DP over the five-count tuple. For the kata's fixed rules, the single-swap pass is the simplest correct algorithm.

## Why the Builder Validates Book Ids

Every other builder field in the F2 sibling katas is "wide open" — any row, any column, any position. Book id is different: the spec rules out anything outside `1..5`, and pushing that rejection into the builder gives tests a stable, one-line way to demonstrate the contract. The exception is `BookOutOfRangeException : ArgumentOutOfRangeException`, matching the `NumberOutOfRangeException` shape the `bingo/` reference established.

The builder accepts negative counts silently (clamped to zero). This is a test-folder affordance — a negative count is malformed but not an interesting domain error; quietly normalising is less ceremony than a second exception type.

## Scenario Map

The twelve scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/KataPotter.Tests/BasketTests.cs`, one `[Fact]` per scenario, test names matching the scenario titles verbatim (modulo C# underscore convention).

## How to Run

```bash
cd kata-potter/csharp
dotnet test
```
