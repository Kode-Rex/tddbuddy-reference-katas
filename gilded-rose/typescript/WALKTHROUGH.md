# Gilded Rose — TypeScript Walkthrough

🚧 Coming in Phase 1.

This walkthrough will provide a commit-by-commit tour of the TypeScript implementation, showing how each scenario from [`../SCENARIOS.md`](../SCENARIOS.md) was driven by tests and how builders, object mothers, and domain types emerged through refactoring.

Each named step below will link to the commit that introduced it:

```
Step 1  — Red: standard items lose one quality per day
Step 2  — Green: minimal pass for scenario 1
Step 3  — Refactor: introduce anItem() builder
Step 4  — Red: standard items lose two quality per day after sell-by
Step 5  — Green
Step 6  — Refactor: extract SellIn and Quality branded types
...
```

The goal is that reading `git log` for this folder reads as a teaching narrative — each commit a meaningful step in the TDD cycle.
