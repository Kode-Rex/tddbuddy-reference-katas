# String Calculator

**Teaching mode:** [Pedagogy](../README.md#1-pedagogy--learn-the-tdd-rhythm) — the first Pedagogy kata in the repo.

Kent Beck's canonical TDD teaching kata. A pure function `add(string) -> int` that starts trivial and grows, one triangulation at a time, into a parser that handles custom delimiters, rejects negatives, and ignores numbers over 1000.

## What this kata teaches

The **rhythm of TDD and the shifting of gears.** Read the commit log end to end and you feel it:

- **Low gear** at the start — fake-it (`return 0`), then triangulate through "one number", "two numbers". Each cycle is a handful of lines. You can see the restraint.
- **Middle gear** once the parser shape is obvious — scenarios land as single red+green commits. The next newline-delimiter scenario doesn't need triangulation; the design already points there.
- **High gear** for the later delimiter variations — any-length, multiple, multi-character multiple. The regex is already in place; new rules drop in as data, not as new code paths.

The walkthroughs narrate *where the gear shifted and why*. That's the teaching.

## Why no builders

The SUT is a pure function: `add(string) -> int`. There is no entity with identity, no collaborator, no state to arrange. A builder on a string would be theatre. Primitive in, primitive out; domain clarity lives in the **test names**, not in setup scaffolding.

Pedagogy katas don't import the Agent-Full-Bake ceremony. When the shape of the problem is a function, the shape of the tests is a function.

## Files

- [`SCENARIOS.md`](./SCENARIOS.md) — the shared spec (9 numbered scenarios, one per requirement).
- [`ARC.md`](./ARC.md) — the intended commit arc all three languages follow.
- `csharp/`, `typescript/`, `python/` — three idiomatic implementations with per-language `WALKTHROUGH.md`.

## How to read

1. Skim `SCENARIOS.md` so you know the target.
2. Pick a language and open its `WALKTHROUGH.md`.
3. Walk the commit table top to bottom. Each row names the cycle phase (red/green/refactor/reflect) and the gear. Click through to see the code at that step.
4. Notice where the gear shifts. That's the moment the solver stopped triangulating and started writing the obvious implementation.
