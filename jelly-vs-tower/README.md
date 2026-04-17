# Jelly vs Tower

Tower defense combat system with color-based damage rules, leveling, and health tracking. Excellent for practicing **test data builders** over a multi-entity domain with a rich rules table.

## What this kata teaches

- **Test Data Builders** — `TowerBuilder` and `JellyBuilder` make scenario setup fluent and readable.
- **Injectable Collaborators** — `RandomSource` is injected so tests control damage rolls without real randomness.
- **Domain Value Types** — `ColorType` as an enum, not a string; damage ranges as typed lookups, not magic numbers.
- **Rules Table Testing** — the damage table is a specification; tests verify the table edges, not just one happy path.
- **Combat Orchestration** — the `Arena` coordinates towers, jellies, damage, and death in a deterministic sequence testable without timing or threading.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
