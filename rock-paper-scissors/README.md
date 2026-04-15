# Rock Paper Scissors

Given two players' plays — `rock`, `paper`, or `scissors` — return the outcome from player 1's perspective: `win`, `lose`, or `draw`.

This kata ships in **Agent Full-Bake** mode at the F1 tier: an algorithmic kata with **no builders**. The domain has two small closed sets — the three plays and the three outcomes — so the function reads as `decide(Play, Play) -> Outcome`. There are no aggregates to construct, no value types beyond the two enums, and no collaborators to inject.

**Typed enums, not strings.** The plays and outcomes are the domain vocabulary, not free text. C# uses `enum Play` / `enum Outcome`; TypeScript uses string-literal unions (`type Play = 'rock' | 'paper' | 'scissors'`); Python uses `StrEnum`. Using primitives here would leak "was it `'Rock'` or `'rock'`?" into every call site — the enum names *are* the ubiquitous language.

The TDD Buddy prompt also proposes a Rock-Paper-Scissors-Spock bonus and an if-statement-free constraint. **Those are out of scope for this reference implementation** — the three-way base game with typed enums is sufficient to demonstrate the F1 shape with named domain sets. See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
