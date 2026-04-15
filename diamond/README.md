# Diamond

Given a letter, render a diamond of letters that starts at `A`, widens to the target letter, then narrows back to `A`. `Print('A')` → `"A"`. `Print('C')` →

```
  A
 B B
C   C
 B B
  A
```

This kata ships in **Agent Full-Bake** mode at the F1 tier: an algorithmic kata with **no builders**. The input is a single `A`–`Z` character and the output is a newline-separated multi-line string — the inputs and outputs *are* the domain, so there are no aggregates to construct, no value types to introduce, and no collaborators to inject. Each implementation exposes one function: `Print(letter)` (C#), `print(letter)` (TS), `print_diamond(letter)` (Python — named to avoid shadowing the Python built-in). See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
