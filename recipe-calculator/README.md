# Recipe Calculator

Scale a recipe by a numeric factor. Given a map of ingredient → quantity and a scale factor, return a new map with every quantity multiplied by the factor.

This kata ships in **Agent Full-Bake** mode at the F1 tier: an algorithmic kata with **no builders**. A recipe is a `Dictionary<string, number>` / `Record<string, number>` / `dict[str, float]` — the inputs and outputs *are* the domain, so there are no aggregates to construct, no value types to introduce, and no collaborators to inject. The teaching point is that scenario-as-test naming still carries when the domain is this thin: each test reads as one line from the spec table.

The TDD Buddy prompt lists bonus tracks for unit conversion, dietary restrictions, ingredient substitution, shopping lists, and nutrition. **Those are out of scope for this reference implementation** — unit conversion is already demonstrated by the sibling [`metric-converter`](../metric-converter/) kata, and the scaling arithmetic alone is sufficient to demonstrate the F1 shape. See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
