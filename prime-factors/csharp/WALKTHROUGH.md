# Prime Factors — C# Walkthrough

This kata ships in **[Pedagogy mode](../../README.md#1-pedagogy--learn-the-tdd-rhythm)**. The commit log is the teaching. Walk it top to bottom and you feel the rhythm: red → green, red → green, refactor, reflect — and the gear shifts from **low** (fake-it, triangulate one data point at a time) to **middle** (one scenario per cycle) to **high** (no new code; later scenarios pass on arrival).

The signature move of Prime Factors is the **run of spec-on-arrival commits** after step 13. That chain is the teaching. Each one says, explicitly, *no new code was needed*. That honesty is the difference between a teaching log and a theater log.

| Step | Commit | Cycle | Gear | What was learned |
|------|--------|-------|------|------------------|
| 1 | [`0a95338`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/0a95338) | scaffold | — | Empty xUnit solution. No SUT yet — the first failing test will name it. |
| 2 | [`800c416`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/800c416) | red | low | First test. Compilation fails because `Factors` doesn't exist. |
| 3 | [`186024e`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/186024e) | green (fake-it) | low | `Array.Empty<int>()`. Restraint is the lesson. |
| 4 | [`f457f42`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/f457f42) | red | low | `generate(2) == [2]`. Second data point. |
| 5 | [`505568f`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/505568f) | green | low | `if (n > 1) return new[] { n };` — still mostly fake, but the guard reads like a rule. |
| 6 | [`b023cea`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/b023cea) | spec (passes on arrival) | low | `generate(3) == [3]` — the `> 1` guard already covers it. Honest pass. |
| 7 | [`9503922`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/9503922) | red | low | `generate(4) == [2, 2]` — first composite. Forces a loop. |
| 8 | [`a331101`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/a331101) | green | low | Divide-out-2 loop: pull `2` while `n % 2 == 0`, then append remaining `n` if `> 1`. The algorithm is starting to emerge. |
| 9 | [`7d2b0d7`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/7d2b0d7) | reflect | low | Empty commit. Pattern is forming — a loop is peeking through. |
| 10 | [`eff72d4`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/eff72d4) | spec (passes on arrival) | low | `generate(6) == [2, 3]` — divide-out-2 reduces to 3; the tail appends. Honest pass. |
| 11 | [`c26d263`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/c26d263) | spec (passes on arrival) | low | `generate(8) == [2, 2, 2]` — the while loop handles powers of two. |
| 12 | [`2c59468`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/2c59468) | red | low → middle | `generate(9) == [3, 3]` — **the hinge.** First odd composite. The divide-out-2 shortcut appends 9 as if it were prime. |
| 13 | [`889f980`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/889f980) | green | middle | `for (divisor = 2; n > 1; divisor++)` wrapping the while. **The algorithm is now complete.** Every subsequent test passes without further code. |
| 14 | [`ed92882`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/ed92882) | reflect | middle | Empty commit. Marks the end of algorithmic pressure and the beginning of proof-by-example. |
| 15 | [`e8dd71c`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/e8dd71c) | spec (passes on arrival) | middle | `generate(12) == [2, 2, 3]`. No new code. |
| 16 | [`e6f7db2`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/e6f7db2) | spec (passes on arrival) | middle | `generate(15) == [3, 5]`. No new code. |
| 17 | [`c13f541`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/c13f541) | spec (passes on arrival) | high | `generate(100) == [2, 2, 5, 5]`. No new code. |
| 18 | [`8fa9a05`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/8fa9a05) | spec (passes on arrival) | high | `generate(30030) == [2, 3, 5, 7, 11, 13]`. Six distinct primes, no new code. |

## How to run

```bash
cd prime-factors/csharp
dotnet test
```

## The takeaway

Two reflect commits. **Seven** spec-on-arrival commits (3, 6, 8, 12, 15, 100, 30030). One real algorithmic breakthrough, at step 13 — the line `for (var divisor = 2; n > 1; divisor++)`. Everything else is the test list proving, one input at a time, that the algorithm already generalized.

That's the difference Prime Factors teaches: **the algorithm can outrun the test list**. Your job is to notice, and to label the commits honestly when it happens.

C# idioms used: `IReadOnlyList<int>` as the public return type, `List<int>` internally, `Array.Empty<int>()` for the allocation-free fake. No LINQ — the imperative loop reads cleaner than `.Aggregate(...)` for this shape.
