# Remaining Katas — Reference Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use `superpowers:subagent-driven-development` (recommended) to implement this plan one kata at a time. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Produce C#/TypeScript/Python reference implementations for the 71 remaining TDD Buddy katas in three distinct **teaching modes** (Pedagogy / Specification / Agent Full-Bake), following the patterns established by `gilded-rose/` (Specification mode) and `bank-account/` (Agent Full-Bake mode), and link each from the astro-site kata page.

**Architecture:** Three teaching modes live side-by-side in the same repo. Five **Pedagogy** katas show the red→green→reflect→refactor arc and gear shifts across many small commits — for humans learning TDD. One **Specification** kata (Gilded Rose, already done) shows commit-per-scenario as executable spec — for teams adopting builders+ubiquitous language. The remaining ~65 **Agent Full-Bake** katas ship as one commit per language with a design-rationale walkthrough — for demonstrating what AI-assisted TDD produces at quality.

**Tech Stack (identical across all katas and modes):**
- **C#** — .NET 8, xUnit, FluentAssertions 6.12.0
- **TypeScript** — Node 20, Vitest 1.6, TypeScript 5 strict
- **Python** — 3.11, pytest, dataclasses, `src/<snake_name>/` layout with `pyproject.toml`

---

## How To Use This Plan

1. Pick a kata from the Classification Table below.
2. Read its spec at `astro-site/src/content/katas/<kata>.md`.
3. Apply the **Template** that matches its mode (P / S / F).
4. Dispatch a subagent with the template's task list + the kata spec + the mode-appropriate reference implementation.
5. Review subagent output, commit, push, move to next kata.

**Do not skip the Classification Table.** The mode dictates commit shape and walkthrough style. A Pedagogy kata shipped as Full-Bake loses the teaching arc; a Full-Bake kata written as Pedagogy bloats the log with theatre.

---

## The Three Teaching Modes

### Mode P — Pedagogy (5 katas)

**Audience:** humans learning TDD.

**Shape:** commits follow the red→green→reflect→refactor cycle, one per step. Gears visibly shift as the solver's confidence grows — early commits are triangulations (fake-it, obvious implementation), later ones collapse scenarios together once the pattern is clear. Walkthrough narrates the reasoning at each commit, including the moments where the solver **chose to shift gears** and why.

**When to use:** a kata whose canonical TDD arc is itself the teaching point (Kent Beck's String Calculator, Uncle Bob's Prime Factors, Bowling Game).

**Included katas:**
1. **string-calculator** — Beck's canonical kata. Starts with "empty string returns 0", triangulates through single/two/many numbers, then custom delimiters, then negatives, then >1000 filter. Classic low-to-high gear progression.
2. **prime-factors** — Martin's 7-line masterpiece. Shows the shift from "write a test, hack it green" to "the algorithm emerges from pure triangulation".
3. **bowling-game** — Martin's Bowling Game. Frame/roll shape emerges through refactor, not up-front design.
4. **tennis-score** — State-machine refactoring of an if/else score tracker. Middle-gear refactor as cleanup is the pedagogy.
5. **roman-numerals** — Triangulation across expanding cases; the subtraction rule (IV, IX) is the "gear-shift" moment.

### Mode S — Specification (1 kata, done)

**Audience:** teams adopting "tests as spec".

**Shape:** one commit per scenario, builders and ubiquitous language from scenario one, no visible refactor noise. Walkthrough is a commit table.

**Included katas:**
1. **gilded-rose** — Already complete.

Mode S is not a template subagents execute — it's here for context. Future specification-mode katas require a dedicated plan.

### Mode F — Agent Full-Bake (~65 katas)

**Audience:** "what does TDD-done-well by AI look like?"

**Shape:** one commit per language, full domain design landing together. Walkthrough reads as design rationale — *why* `Money` is a type, *why* `Clock` is injected, *why* this builder returns a tuple. The test file maps 1:1 to scenarios; the commit log does not pretend to be the journey.

**Most remaining katas go here.** Builder-worthiness still matters (see the tier split below) — a pure algorithm kata does not need builders even in full-bake mode.

**Sub-tiers within Mode F** (determines builder depth, not commit shape):
- **F1** — No builders. Pure algorithms / string transforms. Tests read the kata's inputs and outputs directly. *(25 katas)*
- **F2** — Light builder. One primary entity; a 10–30 line builder earns its keep. *(13 katas)*
- **F3** — Full builders. Rich domain: multiple builders, domain value types, collaborator interfaces, object mothers. *(27 katas)*

---

## Classification Table

### Mode P — Pedagogy (5)

| Kata | Gear Arc | Teaching Point |
|------|----------|----------------|
| string-calculator | Low → Middle → High | Triangulation expanding to regex-parsed delimiters |
| prime-factors | Low → High | Algorithm emerges from fake-it progression |
| bowling-game | Low → Middle | Frame/roll design emerges through refactor |
| tennis-score | Low → Middle | State machine refactored from conditionals |
| roman-numerals | Low → High | Subtraction rule is the gear shift |

### Mode F1 — Full-Bake, no builders (25)

| Kata | Why F1 |
|------|--------|
| 100-doors | Pure math iteration |
| anagram-detector | String comparison |
| balanced-brackets | Stack algorithm |
| change-maker | Coin change over int array |
| conways-sequence | Look-and-say single function |
| diamond | ASCII-art generation |
| end-of-line-trim | String trim |
| fizz-buzz-whiz | Classic; no domain object |
| greeting | String formatting |
| ip-validator | Regex/parsing |
| last-sunday | Date math |
| leap-year | Three rules, boolean |
| linked-list | Data structure; tests exercise operations |
| metric-converter | Conversion table |
| numbers-to-words | Number → string |
| recipe-calculator | Scaling arithmetic |
| rock-paper-scissors | Three-way switch |
| text-justification | String algorithm |
| time-zone-converter | Timezone arithmetic |
| url-parts | URL parsing |
| url-shortener | Map + hash |
| word-wrap | String algorithm |
| fluent-calc | Fluent API is the SUT; no builder needed |
| character-copy | Streaming; Source/Dest are mocks not builders |
| age-calculator | Date delta; no entity worth building |

### Mode F2 — Full-Bake, light builder (13)

| Kata | Builder |
|------|---------|
| calc-refactor | `CalculatorBuilder` (refactoring kata: characterize first) |
| code-breaker | `SecretBuilder` / `GuessBuilder` |
| password | `PolicyBuilder(minLength, requiresDigit, requiresSymbol, ...)` |
| pagination | `PageRequestBuilder(page, size, total)` |
| string-transformer | `PipelineBuilder` |
| tic-tac-toe | `BoardBuilder().WithXAt(r,c).WithOAt(r,c)` |
| timesheet-calc | `TimesheetBuilder().WithEntry(day, hours)` |
| todo-list | `TaskBuilder(title, due, done)` |
| bingo | `CardBuilder` |
| clam-card | `CardBuilder`, `RideBuilder` |
| kata-potter | `BasketBuilder().WithBook(series, count)` |
| mars-rover | `RoverBuilder`, `CommandBuilder` |
| tennis-refactoring | Characterization test set only |

### Mode F3 — Full-Bake, full builders (27)

| Kata | Key Builders / Entities |
|------|-------------------------|
| bank-ocr | `DigitBuilder`, `AccountNumberBuilder` |
| blog-web-app | `PostBuilder`, `UserBuilder`, `CommentBuilder` |
| circuit-breaker | `BreakerBuilder(threshold, timeout, state)` |
| csv-query | `RowBuilder`, `QueryBuilder` |
| event-sourcing | `EventBuilder`, aggregate rebuild from stream |
| expense-report | `EmployeeBuilder`, `ExpenseBuilder` (characterize first) |
| game-of-life | `GridBuilder().WithLivingCellsAt(...)` |
| heavy-metal-bake-sale | `ProductBuilder`, `OrderBuilder` |
| jelly-vs-tower | `TowerBuilder`, `BlockBuilder` |
| laundry-reservation | `ReservationBuilder(slot, service, customer)` |
| library-management | `BookBuilder`, `MemberBuilder`, `LoanBuilder` *(SCENARIOS.md exists)* |
| markdown-parser | `DocumentBuilder`, node builders |
| maze-walker | `MazeBuilder`, `WalkerBuilder` |
| memory-cache | `CacheBuilder(capacity, ttl, clock)` |
| parking-lot | `LotBuilder`, `VehicleBuilder`, `TicketBuilder` |
| poker-hands | `CardBuilder`, `HandBuilder` *(SCENARIOS.md exists)* |
| rate-limiter | `LimiterBuilder(rule, clock)` |
| robot-factory | `RobotBuilder`, `OrderBuilder` |
| shopping-cart | `ProductBuilder`, `LineItemBuilder`, `CartBuilder` *(SCENARIOS.md exists)* |
| snake-game | `BoardBuilder`, `SnakeBuilder` |
| social-network | `UserBuilder`, `PostBuilder`, `NetworkBuilder` |
| supermarket-pricing | `ProductBuilder`, `PricingRulesBuilder` |
| video-club-rental | `UserBuilder`, `TitleBuilder`, `RentalBuilder` *(SCENARIOS.md exists)* |
| weather-station | `ReadingBuilder`, `StationBuilder` |
| zombie-survivor | `SurvivorBuilder`, `HistoryBuilder` |

### Tier 4 — Special (3, out of scope for this plan)

| Kata | Why Special |
|------|-------------|
| multi-threaded-santa | Concurrency primitives — tests need barriers/latches, not builders |
| roll-your-own-mock-framework | Meta-kata: the SUT *is* a mock framework |
| roll-your-own-test-framework | Meta-kata: the SUT *is* a test framework |

Each warrants a dedicated brainstorming session.

---

## File Structure (per kata)

```
<kata>/
├── README.md           — teaches + patterns + teaching mode
├── SCENARIOS.md        — shared spec all three languages satisfy
├── csharp/
│   ├── README.md       — build & run
│   ├── WALKTHROUGH.md  — style depends on mode (see below)
│   ├── .gitignore
│   ├── <Solution>.sln
│   ├── src/<Project>/…
│   └── tests/<Project>.Tests/…
├── typescript/  (parallel layout)
└── python/      (parallel layout)
```

Plus `astro-site/src/content/katas/<kata>.md` updates: frontmatter `solutionsCSharp/TypeScript/Python` + body `## Reference Walkthrough` section.

**Walkthrough styles by mode:**
- **Mode P** — Commit-by-commit narrative; names the gear at each step; flags the red/green/refactor moments and the gear shifts.
- **Mode S** — Commit table; one row per scenario with SHA and link.
- **Mode F** — Design rationale; explains *why* types/builders/collaborators exist.

---

## Templates

### Template P — Pedagogy (5 katas)

**Reference model:** `string-calculator/` (complete). Mirror its shape and conventions.

**Pedagogy-mode style conventions** (established by string-calculator; apply to prime-factors, bowling-game, tennis-score, roman-numerals):

- **Inline literals in the SUT when the walkthrough names them by value.** If the walkthrough row reads "Filter numbers greater than 1000 with `n <= 1000`", the code should show `n <= 1000` inline, not `n <= MaxAddend`. Pulling these into named constants hides the teaching moment. This is the opposite of the rule for Mode F — in a full-bake kata, named constants win.
- **Parse-return shapes may differ per language.** C# may return a tuple of literal strings; TS may return a compiled RegExp; Python may return a NamedTuple with a regex pattern string. Each is idiomatic. Walkthroughs should acknowledge the divergence rather than claim "only idioms differ."
- **Exception types are language-idiomatic.** C# `ArgumentException`, TS `Error`, Python `ValueError`. The message string is *byte-identical* across languages; the exception type is not.
- **Reflect commits are empty** (`git commit --allow-empty`). Content-bearing reflect notes belong in the walkthrough's "what was learned" column, not in a `NOTES.md`.
- **`spec —` commit prefix** labels tests that pass on arrival after a prior triangulation generalized correctly. Two or three per kata is the right cadence — they mark the honest moment the algorithm outran the test list.

**Goal:** show the TDD arc as it actually unfolds, including the reflection step. Commits are small enough that a reader can walk them chronologically and absorb the rhythm.

**Commit shape per language (typical):**
```
scaffold
red: empty input returns zero
green: fake it — return 0
red: single number returns itself
green: parse one int
red: two numbers return sum
green: split on comma
refactor: extract parse+sum helper           ← reflect/refactor step
red: arbitrary count of numbers
green: (already works — delete the test? add the test anyway for spec coverage)
reflect: note that triangulation is complete; shift to middle gear
red+green: custom delimiters                 ← combined now — middle gear
red+green: negative numbers throw
refactor: extract DelimiterParser            ← design emerges
red+green: ignore > 1000
red+green: any-length delimiters
```

For a 10-scenario pedagogy kata, expect **~20 commits per language** (roughly 2x scenarios to account for refactor/reflection commits). That's ~60 commits per Pedagogy kata across three languages, ~300 total for the Pedagogy Five.

**Tasks:**

- [ ] **1. Draft shared spec**

  `<kata>/README.md` — declare mode P, name the teaching arc ("Start from empty input, triangulate to two numbers, then custom delimiters, then negatives..."), point to the Gears section of the repo README.

  `<kata>/SCENARIOS.md` — numbered scenario list. For Pedagogy katas, order matters — the list **is** the curriculum.

- [ ] **2. Build a reference arc plan**

  Before coding any language, draft a `<kata>/ARC.md` that lists the intended commit sequence: for each scenario, what gear it lands at, whether it's a pure red+green or a red+green+refactor, and any reflection notes the walkthrough should capture.

  This plan is the contract all three language implementations follow. They don't have to match commit-for-commit — language idioms differ — but they share the arc.

- [ ] **3. C# implementation**

  Follow the Gilded Rose scaffold pattern (see `gilded-rose/csharp/` for reference, even though Gilded Rose is Mode S — the scaffolding is identical).

  Execute the arc commit-by-commit. Each commit message:
  - `red: <scenario>` / `green: <scenario>` / `refactor: <what>` / `reflect: <note>` as the first word.
  - One-line body naming the insight if any.

  Verify `dotnet test` at each green and refactor commit.

- [ ] **4. TypeScript implementation**

  Mirror step 3. The arc is the same; the TS-specific idioms are documented as they arise (destructuring, discriminated unions if they help).

- [ ] **5. Python implementation**

  Mirror step 3 in Python idiom.

- [ ] **6. Walkthroughs**

  `<kata>/<lang>/WALKTHROUGH.md` for each language: a commit-by-commit narrative. For each commit, explain the *decision* — why this step, what was learned, when the gear shifted. Include commit SHAs and GitHub commit links.

  Use `gilded-rose/csharp/WALKTHROUGH.md` as format reference (table of commits), but expand with a "why this step" column specific to Pedagogy mode.

- [ ] **7. Wire astro-site page**

  Frontmatter: three solution URLs.
  Body: `## Reference Walkthrough` section linking all three language walkthroughs, with a one-line note that this is a **Pedagogy mode** kata and the walkthroughs are the curriculum.

- [ ] **8. Update top-level README**

  Mark Kata Status row ✅ ✅ ✅ with Mode = Pedagogy, Gear = `Low→...` reflecting the arc.

- [ ] **9. Push both repos**

---

### Template F1 — Full-Bake, no builders (25 katas)

**Reference model:** `fizz-buzz-whiz/` (complete). Scaffolding follows the Tier-3 conventions (see `bank-account/`) — just omit the builder.

**F1-specific style conventions** (codified from fizz-buzz-whiz):

- **Inline literals in the SUT** when the function body fits on one screen and the literals *are* the rule. F1's opposite-of-Pedagogy stance (use named constants) applies only when the name adds meaning — not when extracting `FIZZ_DIVISOR = 3` just restates "three" twice. Each F1 walkthrough should include one sentence explicitly documenting this choice so downstream authors don't drift.
- **One source file per language, one test file.** If the algorithm splits naturally (e.g. `urlParts/parse.ts` + `urlParts/format.ts`), two files are acceptable — but if you're reaching for three, step back; you may be in F2 territory.
- **Walkthroughs are a single paragraph.** No design-rationale sections. Point at `SCENARIOS.md` and name anything language-specific that matters (e.g. `CultureInfo.InvariantCulture` for C# locale-safety on int→string).
- **Test class/describe names should match the exported surface**, not the namespace. `describe('say', …)` or `describe('fizzBuzzWhiz', …)` — not `describe('FizzBuzzWhiz.say', …)` unless that namespace actually exists at runtime.

**Commit shape:** one commit per language. Implementation, tests, and walkthrough can all be in that single commit.

**Tasks:**

- [ ] **1. Draft shared spec** — `README.md` (one paragraph: "algorithmic kata, no builders — showcases clean test naming in domain language") + `SCENARIOS.md` (6–12 scenarios).

- [ ] **2. C# implementation** — Scaffold (`dotnet new sln/classlib/xunit`, add FluentAssertions). Implement in `src/<Kata>/`. One `[Fact]` per scenario in `tests/<Kata>.Tests/`. Test names `Snake_case_matching_scenario`. Verify `dotnet test` green. Write short `README.md` + `WALKTHROUGH.md` (walkthrough is a single paragraph explaining "algorithmic — no domain builders — the inputs/outputs are the domain"). Commit.

- [ ] **3. TypeScript implementation** — Scaffold (`npm init`, Vitest). Implement in `src/<camelName>.ts`. Tests in `tests/<camelName>.test.ts`, one `it()` per scenario. Verify `npx vitest run` green. Short README + walkthrough. Commit.

- [ ] **4. Python implementation** — Scaffold (`pyproject.toml`, `src/<snake_name>/`). Implement + tests. Verify `.venv/bin/pytest` green. Short README + walkthrough. Commit.

- [ ] **5. Wire astro-site page** — frontmatter URLs + walkthrough section.

- [ ] **6. Update top-level README** — row with ✅/✅/✅, Mode F, Gear High.

- [ ] **7. Push both repos.**

---

### Template F2 — Full-Bake, light builder (13 katas)

**Reference model:** `password/` (complete).

**Difference from F1:** add a small test-folder builder for the one primary entity. No object mothers, no collaborator interfaces.

**F2-specific style conventions** (codified from password):

- **Builder size target: ~20 lines C#, ~25 lines TS, ~40 lines Python.** The "10–30" original guideline is a C#/TS budget; Python's explicit `self` and type annotations push idiomatic builders to 30–40 lines. Don't golf down Python builders to fit a C# target — keep them idiomatic.
- **Rich return types beat bool.** When validation can produce a list of failures, return a `ValidationResult` (or equivalent) and let tests assert on specific failures. The failure strings are part of the spec — identical across languages.
- **Named constants for business numbers.** F2 is Full-Bake mode (unlike F1's inline-literals policy). `DefaultMinLength = 8` is named, not inline.
- **TS may collapse multi-entity files.** C# and Python typically have one type per file; TS idiom is to colocate small related types in one module. Don't force TS to split — note the divergence in the walkthrough.
- **Scope discipline:** if the kata spec describes an F3-shaped system (multiple collaborators, persistence, external services), scope the F2 reference to its pure-domain core and LOUDLY flag the out-of-scope features as stretch in the README, SCENARIOS, walkthroughs, AND astro-site page.

Otherwise follow F1's tasks 1–7. The builder ships with the tests in the same commit.

---

### Template F3 — Full-Bake, full builders (27 katas)

**Reference model:** `bank-account/` — read this before starting any F3 kata. `gilded-rose/` is Mode S but uses the same builder techniques at Tier-3 depth; worth a look too. `video-club-rental/`, `shopping-cart/`, `library-management/` are also shipped F3 references.

**F3-specific style conventions** (codified after the first three applied F3 katas):

- **Domain-specific exception types** for invariant violations. Don't throw `InvalidOperationException` / `Error` / `RuntimeError` for "book not in catalog" or "no copies available" — name the exception (`BookNotInCatalogException`, `NoCopiesAvailableException`, `LineItemNotFoundException`). Tests can then catch the meaningful type, and a reader sees the domain in the stack trace. This is the opposite of throwing-as-validation; it's *naming the rejection*.
- **Cross-language message strings should match byte-for-byte** even when exception types differ idiomatically per language.
- **Explicit-sweep methods** (like `MarkOverdueRentals`, `ExpireReservations`) get a paragraph in the C# walkthrough explaining the design tradeoff. Tests drive the sweep before exercising blocked operations; this models the production caller's responsibility.

**Tasks:**

- [ ] **1. Draft shared spec**

  `<kata>/README.md` — name the builders, collaborators, and teaching patterns this kata showcases.

  `<kata>/SCENARIOS.md` — full structure:
  - `## Ubiquitous Vocabulary` table
  - `## Domain Rules` section
  - `## Test Scenarios` with subsections and 18–25 numbered scenarios

- [ ] **2. C# implementation**

  Scaffold. Domain types in `src/<Kata>/`:
  - `readonly record struct` for pure value types (Money, Quantity)
  - Classes for entities with identity (Account, Cart)
  - Interfaces for collaborators (IClock, INotifier)

  In `tests/`:
  - `<Collaborator>Fake.cs` per interface (FixedClock, RecordingNotifier)
  - `<Entity>Builder.cs` per builder, fluent, returning tuples when needed

  Tests use builders + fakes. Assertions via FluentAssertions.

  Verify `dotnet test` green.

  Write `csharp/README.md` + `csharp/WALKTHROUGH.md`. Walkthrough is **design rationale** (like `bank-account/csharp/WALKTHROUGH.md`) — not a commit tour.

  Commit.

- [ ] **3. TypeScript implementation** — mirror C# design. `Date` with UTC discipline. Builders in `tests/<Entity>Builder.ts`. Walkthrough is a **delta** from the C# walkthrough ("same design, here's what's idiomatic to TS"). Commit.

- [ ] **4. Python implementation** — mirror C# design. `@dataclass(frozen=True)` for values, `Protocol` for collaborators, `Decimal` for money. Re-export via `src/<snake_name>/__init__.py`. Commit.

- [ ] **5. Wire astro-site page** — frontmatter + walkthrough section. Verify `npx astro build`.

- [ ] **6. Update top-level README** — Mode = Agent Full-Bake, Gear = Middle.

- [ ] **7. Commit walkthroughs if not already in language commits; push both repos.**

Per-kata F3 commit count: **4 commits** in reference-katas (C#, TS, Py, walkthroughs) + **1 commit** in astro-site.

---

## Subagent Dispatch Strategy

**One subagent per kata.** A subagent takes a single kata and executes the full template (all three languages + astro-site + commits + push) in isolation.

**Do not parallelize languages within a kata.** The three language implementations share the SCENARIOS.md spec and walkthroughs cross-reference each other.

**Parallelize katas** — batch 3–5 subagents at a time. Each kata folder is independent; the only shared files are `README.md` (Kata Status row) and `astro-site/src/content/katas/` (different files per kata). Append-friendly.

**Review between batches:**
1. Run the full test matrix in each touched kata — don't trust subagent reports.
2. Skim each WALKTHROUGH.md for accuracy.
3. `npx astro build` after each batch.

**Subagent prompt skeleton:**

```
Implement the <kata-name> reference kata across C#, TypeScript, and Python
in /Users/travisfrisinger/Documents/projects/tddbuddy/tddbuddy-reference-katas.

Spec: astro-site/src/content/katas/<kata-name>.md
Template: docs/plans/2026-04-14-remaining-katas.md § Template <P|F1|F2|F3>
Mode: <Pedagogy | Full-Bake F1 | Full-Bake F2 | Full-Bake F3>

Reference implementations to mirror in shape and style:
  - bank-account/  (Mode F, full-bake middle-gear reference)
  - gilded-rose/   (Mode S, commit-per-scenario reference; useful for builder depth)

Deliver:
  - <kata>/README.md + SCENARIOS.md
  - Three language implementations (all tests green)
  - Three WALKTHROUGH.md files (style per mode — see § Walkthrough styles)
  - astro-site/src/content/katas/<kata>.md updated with solution URLs + walkthrough section
  - Row added to reference-katas README.md Kata Status table
  - Commits per § Template <…> Task 7 commit shape
  - Both repos pushed to origin/main

Do not skip verification runs. Report test counts per language.
```

---

## Recommended Order

1. **Build one Pedagogy kata first** — `string-calculator`. It's the canonical teaching kata and becomes the reference for the other four Pedagogy katas.
2. **Remaining four Pedagogy katas** (`prime-factors`, `bowling-game`, `tennis-score`, `roman-numerals`) — can be dispatched in parallel once string-calculator lands.
3. **The four F3 katas that already have SCENARIOS.md** (`video-club-rental`, `shopping-cart`, `library-management`, `poker-hands`) — highest teaching value after the Pedagogy Five; specs are written.
4. **Remaining F3 katas** (23 katas).
5. **F2 katas** (13 katas).
6. **F1 katas** (25 katas).
7. **Tier 4 specials** (3 katas) — separate plan.

---

## Acceptance Criteria (per kata)

- [ ] `<kata>/SCENARIOS.md` enumerates every behavior from the kata spec
- [ ] Each language folder has source, tests, README, WALKTHROUGH, scaffold files, `.gitignore`
- [ ] All three language test suites green; count matches SCENARIOS.md
- [ ] Walkthrough style matches the kata's mode (pedagogy narrative / specification table / design rationale)
- [ ] `astro-site/src/content/katas/<kata>.md` has three solution URLs + walkthrough section
- [ ] `npx astro build` succeeds
- [ ] Top-level `README.md` Kata Status row updated (✅/✅/✅ + Mode + Gear)
- [ ] Both repos pushed; HEAD advanced on origin/main

---

## Self-Review Notes

- **Spec coverage:** every kata in `astro-site/src/content/katas/` (minus gilded-rose, bank-account, tier-4 trio) is classified above.
- **Placeholder scan:** templates contain concrete commands; modes-specific walkthrough styles are named, not vague.
- **Type consistency:** templates reference entities generically (`<Kata>`, `<Entity>`); per-kata subagents resolve from the spec.
- **Pedagogy claim verification:** the Pedagogy Five are all established canonical TDD teaching katas with published reference arcs (Beck/Martin). Subagents implementing them can cross-reference those external sources in the walkthrough.
