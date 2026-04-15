# Prime Factors

**Teaching mode:** [Pedagogy](../README.md#1-pedagogy--learn-the-tdd-rhythm) — the second Pedagogy kata in the repo.

Uncle Bob's canonical triangulation kata. A pure function `generate(int) -> list<int>` that starts returning `[]` for `1`, grows through a few triangulations, and — around test four — collapses into a complete algorithm. Every subsequent test case passes without a code change.

## What this kata teaches

The **moment the algorithm outruns the test list.** Read the commit log end to end and you feel it:

- **Low gear** at the start — fake-it (`return []`), then one data point at a time: `2 -> [2]`, `3 -> [3]`. Each cycle is a handful of lines.
- **The hinge** — `4 -> [2, 2]` forces a loop. `6 -> [2, 3]` forces an outer divisor. Once those two triangulations land, the algorithm is *done*.
- **High gear** for every test that follows — `8`, `9`, `12`, `15`, `100`, `30030`. Each one is labelled `spec —` in the commit log. **No new code.** That honesty is the whole point.

The walkthroughs narrate *when the algorithm became complete* and mark the chain of `spec —` commits that prove it.

## Why no builders

The SUT is a pure function: `generate(int) -> list<int>`. There is no entity with identity, no collaborator, no state to arrange. A builder on an integer would be theatre. Primitive in, list out; domain clarity lives in the **test names**, not in setup scaffolding.

Pedagogy katas don't import the Agent-Full-Bake ceremony. When the shape of the problem is a function, the shape of the tests is a function.

## Files

- [`SCENARIOS.md`](./SCENARIOS.md) — the shared spec (11 numbered scenarios, one per test-case row).
- [`ARC.md`](./ARC.md) — the intended commit arc all three languages follow.
- `csharp/`, `typescript/`, `python/` — three idiomatic implementations with per-language `WALKTHROUGH.md`.

## How to read

1. Skim `SCENARIOS.md` so you know the target.
2. Pick a language and open its `WALKTHROUGH.md`.
3. Walk the commit table top to bottom. Each row names the cycle phase (red / green / refactor / reflect / spec) and the gear. Click through to see the code at that step.
4. Notice the block of consecutive `spec —` rows near the end. That's the moment the algorithm generalized. Every test after the hinge is a proof by example.
