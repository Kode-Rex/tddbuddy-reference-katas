# TDD Buddy — Reference Kata Implementations

Teaching implementations of selected [TDD Buddy](https://www.tddbuddy.com) katas, showcasing the disciplines that make tests function as specifications for humans and AI agents alike:

- **Test Data Builders** — composable, fluent APIs for constructing scenarios
- **Object Mothers** — canonical named instances of domain concepts
- **Scenario Factories** — preconfigured worlds that tests tweak from a coherent baseline
- **Ubiquitous Language** — vocabulary that flows end to end, from business conversation to test name to domain type
- **Domain Types** — `Money`, `SKU`, `Quantity`, not `decimal`, `string`, `int`
- **Mocks as Behavioral Specifications** — used only where collaboration *is* the behavior

These aren't the smallest solutions that pass the tests. They're the solutions that demonstrate how to make tests serve as the product surface of your codebase — the artifact that humans, new hires, and AI agents all operate against.

## Gears — Bridging TDD and BDD

A misconception that dogs TDD discussions: "TDD means tiny red-green-refactor cycles, one assertion at a time." That's one gear. TDD is many gears.

Kent Beck's framing: you **shift gears** based on confidence.

- **Low gear** — fake-it, triangulation, one assertion at a time. Use this when the territory is unfamiliar, the design is uncertain, or a bug is hiding and you need the smallest reliable step.
- **Middle gear** — one scenario per cycle. A test drives a complete behavior, not a single branch. Most of a mature codebase lives here.
- **High gear** — write the whole obvious implementation. Used when the path is clear, the pattern is familiar from neighboring code, or you're porting a well-specified solution to a new language.

**This is where TDD meets BDD.** BDD asks you to write executable specifications at the level of *behavior*, not *branches*. That's middle/high gear. It doesn't replace TDD — it's the gear you shift into once the domain is understood well enough that each scenario *is* a single step.

**The techniques don't change between gears.** Test data builders, ubiquitous language, domain types, mocks-as-specifications — all apply identically at every tempo. What changes is how many scenarios you take in one stride.

Picking the wrong gear is its own failure mode. Low gear on a solved problem is theatre. High gear on an unfamiliar domain is reckless. The skill is knowing when to shift.

## Three Teaching Modes

This repo answers three different questions, and each kata is written in the mode that fits:

### 1. Pedagogy — learn the TDD rhythm

For humans who want to **feel** the TDD cycle: red → green → reflect → refactor, with gears visibly shifting as understanding accrues. Commits land one per cycle, not one per scenario. Walkthroughs narrate *why* each step was taken and when the gear shifted up.

These are the classic TDD teaching katas — small enough that the whole arc fits in the reader's head, rich enough that real design choices emerge.

**Included:** `string-calculator`, `prime-factors`, `bowling-game`, `tennis-score`, `roman-numerals` (five-kata pedagogy set).

### 2. Specification — tests as the system's surface

For teams who want to see **what tests look like when they are the spec**: one commit per scenario, test names are domain sentences, builders and ubiquitous language from scenario one. No visible refactor churn — the design is already understood; each commit adds one behavior.

This is what a mature team's tests read like after the design has settled. It's also what an AI agent operating against your tests should experience.

**Included:** `gilded-rose` (low gear, commit-per-scenario).

### 3. Agent Full-Bake — how we do it with AI

For the question "what does TDD-done-well-by-an-AI look like?": one commit per language, full domain design landing together, walkthrough explaining the design rationale. The tests are still the spec — but the commit history doesn't pretend to be the learning journey.

This is the honest shape of AI-assisted TDD: the agent understands the domain from the kata brief, writes the whole thing to the quality bar, and documents *why* it came out that shape. The commit log is a delivery log, not a learning log.

**Included:** `bank-account`, and the vast majority of remaining katas.

**You can tell which mode a kata is in by its walkthrough.** Pedagogy walkthroughs step through cycles and show gear shifts. Specification walkthroughs table-of-commits. Full-bake walkthroughs read as design rationale.

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
│   ├── src/           — implementation project
│   └── tests/         — test project
├── typescript/
│   ├── README.md
│   ├── WALKTHROUGH.md
│   ├── src/
│   └── tests/
└── python/
    ├── README.md
    ├── WALKTHROUGH.md
    ├── src/
    └── tests/
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

Mode tells you what the kata teaches; gear tells you the rhythm of its commits.

| Kata | C# | TS | Py | Mode | Gear | Notes |
|------|----|----|----|------|------|-------|
| [Gilded Rose](gilded-rose/) | ✅ | ✅ | ✅ | Specification | Low | Commit-per-scenario; builders from day one; legacy-style domain rules |
| [Bank Account](bank-account/) | ✅ | ✅ | ✅ | Agent Full-Bake | Middle | Money value type, injected clock, AccountBuilder |
| [String Calculator](string-calculator/) | ✅ | ✅ | ✅ | Pedagogy | Low→High | Kent Beck's canonical TDD teaching kata |
| [Prime Factors](prime-factors/) | ✅ | ✅ | ✅ | Pedagogy | Low→High | Uncle Bob's triangulation masterclass |
| [Bowling Game](bowling-game/) | ✅ | ✅ | ✅ | Pedagogy | Low→Middle | Frames emerge through refactor — the class that does not get written |
| [Tennis Score](tennis-score/) | ✅ | ✅ | ✅ | Pedagogy | Low→Middle | State machine refactored from if/else chain |
| [Roman Numerals](roman-numerals/) | ✅ | ✅ | ✅ | Pedagogy | Low→High | Table of (value, symbol) pairs beats special cases |
| [Video Club Rental](video-club-rental/) | ✅ | ✅ | ✅ | Agent Full-Bake | Middle | Rich domain, multiple collaborators |
| [Shopping Cart](shopping-cart/) | ✅ | ✅ | ✅ | Agent Full-Bake | Middle | Cart + line items + pricing strategies (Strategy pattern) |
| [Library Management](library-management/) | ✅ | ✅ | ✅ | Agent Full-Bake | Middle | Multi-entity domain, reservation queues |
| [Poker Hands](poker-hands/) | ✅ | ✅ | ✅ | Agent Full-Bake | Middle | Ranking + comparison; builders make hand literals readable |
| [FizzBuzz Whiz](fizz-buzz-whiz/) | ✅ | ✅ | ✅ | Agent Full-Bake | High | Classic FizzBuzz; no builders, pure function |
| [Leap Year](leap-year/) | ✅ | ✅ | ✅ | Agent Full-Bake | High | Divisible by 4 / 100 / 400 rule cascade |
| [100 Doors](100-doors/) | ✅ | ✅ | ✅ | Agent Full-Bake | High | Simulation; perfect-square insight in walkthrough |
| [Rock Paper Scissors](rock-paper-scissors/) | ✅ | ✅ | ✅ | Agent Full-Bake | High | Typed enums for plays and outcomes |
| [Greeting](greeting/) | ✅ | ✅ | ✅ | Agent Full-Bake | High | Progressive string formatting: null stand-in, shouts, Oxford comma, mixed split |
| [Anagram Detector](anagram-detector/) | ✅ | ✅ | ✅ | Agent Full-Bake | High | Pair detection, find-in-list, and group anagrams over a shared sorted-letters key |
| [Balanced Brackets](balanced-brackets/) | ✅ | ✅ | ✅ | Agent Full-Bake | High | Stack-based bracket validation |
| [Conway's Sequence](conways-sequence/) | ✅ | ✅ | ✅ | Agent Full-Bake | High | Look-and-say digit-run expansion |
| [Diamond](diamond/) | ✅ | ✅ | ✅ | Agent Full-Bake | High | Letter-diamond ASCII rendering |
| [End of Line Trim](end-of-line-trim/) | ✅ | ✅ | ✅ | Agent Full-Bake | High | Per-line trailing whitespace removal |
| [IP Validator](ip-validator/) | ✅ | ✅ | ✅ | Agent Full-Bake | High | IPv4 dotted-quad validation |
| [Last Sunday](last-sunday/) | ✅ | ✅ | ✅ | Agent Full-Bake | High | Last Sunday of month/year |
| [Age Calculator](age-calculator/) | ✅ | ✅ | ✅ | Agent Full-Bake | High | Age from birthdate + reference date |
| [Metric Converter](metric-converter/) | ✅ | ✅ | ✅ | Agent Full-Bake | High | Unit conversion with typed enums |
| [Change Maker](change-maker/) | ✅ | ✅ | ✅ | Agent Full-Bake | High | Greedy change-making for canonical coins |
| [Numbers to Words](numbers-to-words/) | ✅ | ✅ | ✅ | Agent Full-Bake | High | Integer → English word phrase |
| [Recipe Calculator](recipe-calculator/) | ✅ | ✅ | ✅ | Agent Full-Bake | High | Scale ingredient quantities by a factor |
| [Text Justification](text-justification/) | ✅ | ✅ | ✅ | Agent Full-Bake | High | Justify text to target width |
| [Time Zone Converter](time-zone-converter/) | ✅ | ✅ | ✅ | Agent Full-Bake | High | Convert times between zones |
| [URL Parts](url-parts/) | ✅ | ✅ | ✅ | Agent Full-Bake | High | Parse URL into components |
| [URL Shortener](url-shortener/) | ✅ | ✅ | ✅ | Agent Full-Bake | High | Bidirectional URL ↔ short-code map |
| [Word Wrap](word-wrap/) | ✅ | ✅ | ✅ | Agent Full-Bake | High | Wrap at word boundaries to target width |
| [Linked List](linked-list/) | ✅ | ✅ | ✅ | Agent Full-Bake | High | Linked list data structure |
| [Fluent Calc](fluent-calc/) | ✅ | ✅ | ✅ | Agent Full-Bake | High | Calculator with fluent chained API |
| [Character Copy](character-copy/) | ✅ | ✅ | ✅ | Agent Full-Bake | High | Source→Destination streaming with injected collaborators |
| [Password](password/) | ✅ | ✅ | ✅ | Agent Full-Bake | Middle | Policy validation with a fluent PolicyBuilder |
| [Pagination](pagination/) | ✅ | ✅ | ✅ | Agent Full-Bake | Middle | PageRequest with derived metadata + PageRequestBuilder |
| [Todo List](todo-list/) | ✅ | ✅ | ✅ | Agent Full-Bake | Middle | TodoList aggregate with TaskBuilder |
| [Tic-Tac-Toe](tic-tac-toe/) | ✅ | ✅ | ✅ | Agent Full-Bake | Middle | Board with BoardBuilder; Outcome detection |
| [Bingo](bingo/) | ✅ | ✅ | ✅ | Agent Full-Bake | Middle | Card marking with win detection |
| [Clam Card](clam-card/) | ✅ | ✅ | ✅ | Agent Full-Bake | Middle | Transit card with zone-based fares |
| [Kata Potter](kata-potter/) | ✅ | ✅ | ✅ | Agent Full-Bake | Middle | Set-discount pricing with adjustment pass |
| [Mars Rover](mars-rover/) | ✅ | ✅ | ✅ | Agent Full-Bake | Middle | Command interpreter with direction enum |
| [Code Breaker](code-breaker/) | ✅ | ✅ | ✅ | Agent Full-Bake | Middle | Mastermind feedback with duplicate-color handling |
| [String Transformer](string-transformer/) | ✅ | ✅ | ✅ | Agent Full-Bake | Middle | Pipeline of string transformations |
| [Timesheet Calc](timesheet-calc/) | ✅ | ✅ | ✅ | Agent Full-Bake | Middle | Timesheet entries with overtime calculation |
| [Calc Refactor](calc-refactor/) | ✅ | ✅ | ✅ | Agent Full-Bake | Middle | Refactoring kata; characterization tests + clean implementation |
| [Tennis Refactoring](tennis-refactoring/) | ✅ | ✅ | ✅ | Agent Full-Bake | Middle | Characterization-test refactor of legacy tennis scorer |
| [Memory Cache](memory-cache/) | ✅ | ✅ | ✅ | Agent Full-Bake | Middle | Bounded TTL cache with LRU eviction |
| [Circuit Breaker](circuit-breaker/) | ✅ | ✅ | ✅ | Agent Full-Bake | Middle | State machine with failure threshold + timeout |
| *(60+ more)* | — | — | — | Agent Full-Bake | Middle/High | See [`docs/plans/2026-04-14-remaining-katas.md`](docs/plans/2026-04-14-remaining-katas.md) |

More katas will be added as the reference set grows. Algorithmic katas like Prime Factors and Roman Numerals appear here in **Pedagogy mode** — the algorithm-emerges-from-triangulation arc is itself the teaching point. Domain-heavy katas appear in **Agent Full-Bake mode** with builders and ubiquitous language. Straightforward one-liner solutions for every TDD Buddy kata in many languages live in the main [tddbuddy-solutions](https://github.com/Kode-Rex/tddbuddy-solutions) repo.

## Contributing

This is a teaching repo. Pull requests that add a new kata, improve a walkthrough, or translate an existing kata into a new language are welcome. The bar:

- Scenarios are shared across all three languages (if adding a new kata, start with `SCENARIOS.md`)
- Test names use domain language, not implementation language
- Setup uses builders; primitives only where a primitive is the domain concept
- Commit history reads as a teaching narrative

## License

MIT — see [LICENSE](LICENSE).
