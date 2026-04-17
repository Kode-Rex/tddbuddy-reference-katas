# Zombie Survivor

Model survivors in a zombie-apocalypse boardgame. This multi-step kata builds complexity incrementally — survivors, equipment, game management, experience/leveling, history logging, and skill trees. Excellent for practicing **test data builders** across a rich, evolving domain.

## What this kata teaches

- **Test Data Builders** — `SurvivorBuilder` and `HistoryBuilder` make multi-step game scenarios one-line setups.
- **State Progression** — survivors move through wound states and experience levels; the game tracks the highest level among living survivors.
- **Domain-Specific Exceptions** — invariant violations like duplicate names and equipment overflow get named exception types, not generic throws.
- **Event History** — the game logs domain events (survivor added, wound received, level up, etc.) as an append-only history.
- **Skill Trees** — level-gated skill unlocks with mandatory first picks and cycling.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
