# Code Breaker

A Mastermind-style feedback engine. Given a 4-peg **secret** and a 4-peg **guess**, return **feedback** that names exact matches (correct peg in the correct position) and color-only matches (correct peg in the wrong position) — with duplicate-peg handling that does not double-count.

This kata ships in **Agent Full-Bake** mode at the **F2 tier**: two small test-folder builders (`SecretBuilder`, `GuessBuilder`) make four-peg code literals readable in tests, a typed `Peg` enum names the six playable values, and a `Feedback` result carries both the structured counts (`exactMatches`, `colorMatches`) and the canonical `"+"`/`"-"` string that the spec uses.

## Scope — Feedback Engine Only

The original TDD Buddy prompt on the astro-site names three bonus directions: configurable code length, configurable digit range, and a full game loop that generates a random secret and tracks attempts. **The game loop and random-secret generation are out of scope for this reference.** Introducing them would add a `SecretGenerator` collaborator and an attempts-tracking aggregate — F3 territory.

This reference is scoped to **feedback scoring only**: given a secret and a guess (both 4 pegs drawn from the digits 1–6), compute the feedback.

### Stretch Goals (Not Implemented Here)

These are deliberately left for a follow-up:

- **Random secret generation** — a `SecretGenerator` collaborator driving `Game.Start()`.
- **Game loop** — accept guesses, track attempt count, detect win/loss.
- **Configurable code length** — 4, 5, or 6 pegs.
- **Configurable peg range** — 1–8 instead of 1–6.

Each belongs alongside the feedback engine, but each introduces its own collaborators or parameterizations; together they tip the kata into F3. See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification this reference does satisfy.
