# String Transformer

A **pipeline** runs an ordered sequence of string **transformations** — capitalise, reverse, snake-case, truncate, repeat, replace, and friends — and returns the final transformed string. Each transformation is a tiny strategy object implementing a uniform `Apply(string) -> string` contract; the pipeline wires them together; the `PipelineBuilder` names each step so tests read as declarative setup.

This kata ships in **Agent Full-Bake** mode at the **F2 tier**. The teaching point is two-fold: the **Strategy pattern** (every transformation is a self-contained unit swappable without touching the pipeline) and the **light builder payoff** (a fluent `PipelineBuilder` keeps each scenario to one readable line that names only the transformations it cares about).

## Scope — The Eight Required Transformations

The astro-site spec enumerates **eight chainable operations** plus bonus ideas. This reference implements exactly the eight required operations; the bonus operations are explicitly out of scope.

**In scope:**

- `capitalise()` — capitalise the first letter of each whitespace-separated word
- `reverse()` — reverse the entire string
- `removeWhitespace()` — remove every whitespace character
- `snakeCase()` — convert to `snake_case` (hyphens and whitespace become underscores, runs collapse)
- `camelCase()` — convert to `camelCase` (first word lowercase, subsequent word-initials uppercase, separators removed)
- `truncate(n)` — truncate to `n` characters, appending `"..."` if truncation occurred
- `repeat(n)` — produce the string repeated `n` times, joined by a single space
- `replace(target, replacement)` — replace every occurrence of the literal target substring

### Stretch Goals (Not Implemented Here)

Left for a follow-up; each keeps the F2 shape intact but would triple the scenario count:

- **`cipher(n)`** — Caesar cipher shifting each letter by `n` positions
- **`slug()`** — URL-friendly slug (lowercase, hyphens, strip non-alphanumerics)
- **`mask(n)`** — keep the last `n` characters visible, replace the rest with `*`
- **Immutable branching** — each operation returns a new `Pipeline`, so a single base pipeline can fork into divergent transformations

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification this reference satisfies.
