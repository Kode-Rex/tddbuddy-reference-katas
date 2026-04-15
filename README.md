# TDD Buddy — Reference Kata Implementations

Teaching implementations of selected [TDD Buddy](https://www.tddbuddy.com) katas, showcasing the disciplines that make tests function as specifications for humans and AI agents alike:

- **Test Data Builders** — composable, fluent APIs for constructing scenarios
- **Object Mothers** — canonical named instances of domain concepts
- **Scenario Factories** — preconfigured worlds that tests tweak from a coherent baseline
- **Ubiquitous Language** — vocabulary that flows end to end, from business conversation to test name to domain type
- **Domain Types** — `Money`, `SKU`, `Quantity`, not `decimal`, `string`, `int`
- **Mocks as Behavioral Specifications** — used only where collaboration *is* the behavior

These aren't the smallest solutions that pass the tests. They're the solutions that demonstrate how to make tests serve as the product surface of your codebase — the artifact that humans, new hires, and AI agents all operate against.

## Why This Repo Exists

The [TDD Buddy blog](https://www.tddbuddy.com/blog) argues — across a three-part arc — that the bar for TDD has moved. Agents shifted the audience for tests from humans-with-context to agents-without, and that raised what "good tests" means:

1. [TDD Already Does BDD — Without the Gherkin](https://www.tddbuddy.com/blog/tdd-already-does-bdd/) — the craft case
2. [BDD Was a Coordination Tax — AI Just Repriced It](https://www.tddbuddy.com/blog/bdd-was-a-coordination-tax/) — the org case
3. [The Bar for TDD Just Moved](https://www.tddbuddy.com/blog/the-bar-for-tdd-just-moved/) — the new floor

This repo is the concrete companion. Every kata here applies the techniques the arc describes.

## Layout

Each kata has:

```
<kata-slug>/
├── README.md          — what this kata teaches, which patterns it showcases
├── SCENARIOS.md       — the scenarios all three languages implement (the shared spec)
├── csharp/
│   ├── README.md      — how to build and run
│   ├── WALKTHROUGH.md — commit-by-commit teaching walkthrough
│   └── <solution/tests>
├── typescript/
│   ├── README.md
│   ├── WALKTHROUGH.md
│   └── <src/tests>
└── python/
    ├── README.md
    ├── WALKTHROUGH.md
    └── <src/tests>
```

**Same scenarios. Same vocabulary. Three idiomatic implementations.**

The test names, domain concepts, and scenario structure are identical across languages. Only the language-specific syntax differs. This is deliberate — the vocabulary of the business should not depend on which programming language a team happens to be using.

## How to Read a Kata

1. Start with the kata's top-level `README.md` — the patterns being taught and why this kata showcases them.
2. Read `SCENARIOS.md` — the shared specification. This is what all three implementations satisfy.
3. Pick your language and read its `WALKTHROUGH.md` — a commit-by-commit tour of how the solution was built, with links to the commits themselves.
4. Read the final code. Each file is written to be read — named in domain language, small in scope, composable through builders and factories.

## Reading the Commit History

Each language folder has a meaningful commit history. Reading `git log` for any language folder should read like a narrative of how TDD actually unfolds:

```
commit 12 — Refactor: extract ItemCategory to replace stringly-typed categories
commit 11 — Green: loyalty discount applies to legendary items correctly
commit 10 — Red: loyalty members get discount on legendary items
commit 09 — Refactor: extract aLegendaryItem() builder
commit 08 — Green: legendary items do not degrade
commit 07 — Red: legendary items do not degrade in quality
commit 06 — Refactor: introduce Item builder with sensible defaults
...
```

The walkthrough doc links each named step to its commit so the reader can check out any point in the TDD cycle and see exactly what the code looked like.

## Katas Included (Growing Over Time)

| Kata | C# | TypeScript | Python | Notes |
|------|----|-----------|--------|-------|
| [Gilded Rose](gilded-rose/) | 🚧 | 🚧 | 🚧 | First reference kata — classic refactoring + rich domain rules |

More katas will be added as the reference set grows. Not every kata in the TDD Buddy catalog will get this treatment — only those where scenario-heavy, domain-rich design makes builders and ubiquitous language genuinely pay off. Algorithmic katas (FizzBuzz, Prime Factors, Roman Numerals) are covered by the main [tddbuddy-solutions](https://github.com/Kode-Rex/tddbuddy-solutions) repo as straightforward solutions.

## Contributing

This is a teaching repo. Pull requests that add a new kata, improve a walkthrough, or translate an existing kata into a new language are welcome. The bar:

- Scenarios are shared across all three languages (if adding a new kata, start with `SCENARIOS.md`)
- Test names use domain language, not implementation language
- Setup uses builders; primitives only where a primitive is the domain concept
- Commit history reads as a teaching narrative

## License

MIT — see [LICENSE](LICENSE).
