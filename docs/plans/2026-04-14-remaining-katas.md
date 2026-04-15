# Remaining Katas — Reference Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use `superpowers:subagent-driven-development` (recommended) to implement this plan one kata at a time. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Produce C#/TypeScript/Python reference implementations for the 71 remaining TDD Buddy katas, following the patterns set by `gilded-rose/` (low gear) and `bank-account/` (middle gear), and link each from the astro-site kata page.

**Architecture:** Each kata follows the same directory shape: `<kata>/{README.md, SCENARIOS.md, csharp/, typescript/, python/}`. Per-kata effort is scoped by **gear** (low/middle/high) and **builder-worthiness** — not every kata warrants the full test-data-builder treatment. Classification lives in this plan. Subagents implement one kata end-to-end per dispatch.

**Tech Stack (per language, identical across katas):**
- **C#** — .NET 8, xUnit, FluentAssertions 6.12.0
- **TypeScript** — Node 20, Vitest 1.6, TypeScript 5 strict
- **Python** — Python 3.11, pytest, dataclasses, `src/<snake_name>/` layout with `pyproject.toml`

---

## How To Use This Plan

1. Pick a kata from the Classification Table below.
2. Read the kata's spec at `astro-site/src/content/katas/<kata>.md`.
3. Apply the **Template** that matches its tier (A, B, C, or D).
4. Dispatch a subagent with the template's task list + the kata spec + the tier-appropriate reference implementation (`gilded-rose/` for low gear, `bank-account/` for middle gear).
5. Review subagent output, commit, push, move to next kata.

**Do not skip the Classification Table.** Gear + builder choice drives the template. A low-gear kata run through the middle-gear template is wasted effort; a middle-gear kata run through the low-gear template loses the teaching point.

---

## Classification Table

**Tier 1 — Low gear, no builders** (pure algorithms / string transforms, ≤10 tests typical, one file each)

| Kata | Why Tier 1 |
|------|-----------|
| 100-doors | Pure math iteration |
| anagram-detector | String comparison |
| balanced-brackets | Stack algorithm |
| change-maker | Coin change, array of ints |
| conways-sequence | Look-and-say, single function |
| diamond | ASCII art, single function |
| end-of-line-trim | String trim |
| fizz-buzz-whiz | Classic; no domain object |
| greeting | String formatting |
| ip-validator | Regex/parsing |
| last-sunday | Date math |
| leap-year | Three rules, boolean |
| linked-list | Data structure; its own type, but tests exercise operations not variations |
| metric-converter | Conversion table |
| numbers-to-words | Number → string |
| prime-factors | Classic; integer → list |
| recipe-calculator | Scaling arithmetic |
| rock-paper-scissors | Three-way switch |
| roman-numerals | Classic |
| string-calculator | Kent Beck's canonical TDD kata |
| text-justification | String algorithm |
| time-zone-converter | Timezone arithmetic |
| url-parts | URL parsing |
| url-shortener | Map + hash |
| word-wrap | String algorithm |

**Tier 2 — Low gear, light builder** (one primary entity, a few meaningful variations; a small builder helps but not the whole cathedral)

| Kata | Primary Entity / Builder |
|------|--------------------------|
| age-calculator | `PersonBuilder(birthdate, referenceDate)` |
| bowling-game | `GameBuilder().WithRoll(n).WithStrike().WithSpare()` |
| calc-refactor | Refactoring kata — characterization tests first, no builder |
| character-copy | Streaming; `Source`/`Dest` mocks |
| code-breaker | `SecretBuilder`/`GuessBuilder` |
| fluent-calc | Fluent API is the SUT; no builder needed |
| password | `PolicyBuilder(minLength, requiresDigit, requiresSymbol, ...)` |
| pagination | `PageRequestBuilder(page, size, total)` |
| string-transformer | `PipelineBuilder` |
| tennis-score | `GameBuilder().WithPointsFor("player1", n)` |
| tic-tac-toe | `BoardBuilder().WithXAt(r, c).WithOAt(r, c)` |
| timesheet-calc | `TimesheetBuilder().WithEntry(day, hours)` |
| todo-list | `TaskBuilder(title, due, done)` |

**Tier 3 — Middle gear, full builders** (rich domain; full ubiquitous language + test data builders + object mothers where useful)

| Kata | Key Builders / Entities |
|------|-------------------------|
| bank-ocr | `DigitBuilder`, `AccountNumberBuilder` |
| bingo | `CardBuilder`, `DrawerBuilder` |
| blog-web-app | `PostBuilder`, `UserBuilder`, `CommentBuilder` |
| circuit-breaker | `BreakerBuilder(threshold, timeout, state)` |
| clam-card | `CardBuilder`, `RideBuilder` |
| csv-query | `RowBuilder`, `QueryBuilder` |
| event-sourcing | `EventBuilder`, aggregate rebuild from event stream |
| expense-report | `EmployeeBuilder`, `ExpenseBuilder` (refactoring kata: characterize first) |
| game-of-life | `GridBuilder().WithLivingCellsAt(...)` |
| heavy-metal-bake-sale | `ProductBuilder`, `OrderBuilder` |
| jelly-vs-tower | `TowerBuilder`, `BlockBuilder` |
| kata-potter | `BasketBuilder().WithBook(series, count)` |
| laundry-reservation | `ReservationBuilder(slot, service, customer)` |
| library-management | `BookBuilder`, `MemberBuilder`, `LoanBuilder` (SCENARIOS.md already exists) |
| markdown-parser | `DocumentBuilder`, node builders |
| mars-rover | `RoverBuilder`, `GridBuilder`, `CommandBuilder` |
| maze-walker | `MazeBuilder`, `WalkerBuilder` |
| memory-cache | `CacheBuilder(capacity, ttl, clock)` |
| parking-lot | `LotBuilder`, `VehicleBuilder`, `TicketBuilder` |
| poker-hands | `CardBuilder`, `HandBuilder` (SCENARIOS.md already exists) |
| rate-limiter | `LimiterBuilder(rule, clock)` |
| robot-factory | `RobotBuilder`, `OrderBuilder` |
| shopping-cart | `ProductBuilder`, `LineItemBuilder`, `CartBuilder` (SCENARIOS.md already exists) |
| snake-game | `BoardBuilder`, `SnakeBuilder` |
| social-network | `UserBuilder`, `PostBuilder`, `NetworkBuilder` |
| supermarket-pricing | `ProductBuilder`, `PricingRulesBuilder` |
| tennis-refactoring | Refactoring kata — characterization tests first |
| video-club-rental | `UserBuilder`, `TitleBuilder`, `RentalBuilder` (SCENARIOS.md already exists) |
| weather-station | `ReadingBuilder`, `StationBuilder` |
| zombie-survivor | `SurvivorBuilder`, `HistoryBuilder` |

**Tier 4 — Special** (meta/concurrency; each needs its own sub-plan)

| Kata | Why Special |
|------|-------------|
| multi-threaded-santa | Concurrency primitives — tests need barriers/latches, not builders |
| roll-your-own-mock-framework | Meta-kata: the SUT *is* a mock framework |
| roll-your-own-test-framework | Meta-kata: the SUT *is* a test framework |

**Tier 4 plans are out of scope for this document.** Each warrants a dedicated brainstorming session before implementation.

---

## File Structure (per kata)

```
<kata>/
├── README.md           — what the kata teaches + which patterns it showcases
├── SCENARIOS.md        — shared spec all three languages satisfy
├── csharp/
│   ├── README.md       — build & run
│   ├── WALKTHROUGH.md  — design rationale (middle gear) or commit tour (low gear)
│   ├── .gitignore
│   ├── <Solution>.sln
│   ├── src/<Project>/…
│   └── tests/<Project>.Tests/…
├── typescript/
│   ├── README.md
│   ├── WALKTHROUGH.md
│   ├── .gitignore
│   ├── package.json
│   ├── tsconfig.json
│   ├── vitest.config.ts
│   ├── src/…
│   └── tests/…
└── python/
    ├── README.md
    ├── WALKTHROUGH.md
    ├── .gitignore
    ├── pyproject.toml
    ├── src/<snake_name>/…
    └── tests/…
```

Additionally, each kata updates `astro-site/src/content/katas/<kata>.md`:
- Frontmatter gets `solutionsCSharp`, `solutionsTypeScript`, `solutionsPython` URLs
- Body gets a `## Reference Walkthrough` section linking all three walkthroughs

---

## Templates

### Template A — Tier 1 (Low gear, no builders)

**Goal:** pure-algorithm kata as three thin language implementations. One source file per language, one test file, no builders, no domain types beyond the primitive inputs/outputs the algorithm operates on.

**Reference:** there is no prior Tier-1 example in this repo yet — this template stands on its own. Follow the Tier-2/3 stack conventions; just omit builders.

**Tasks:**

- [ ] **1. Draft shared spec**

  Create `<kata>/README.md` (short — what it teaches, what it doesn't: "algorithmic kata, no builders — showcases clean test naming in domain language").

  Create `<kata>/SCENARIOS.md` with a numbered scenario list (typically 6–12 scenarios). Scenario titles must be full sentences that will map 1:1 to test names.

- [ ] **2. C# implementation**

  Scaffold:
  ```bash
  cd <kata>/csharp
  dotnet new sln -n <Kata>
  dotnet new classlib -n <Kata> -o src/<Kata> -f net8.0
  dotnet new xunit -n <Kata>.Tests -o tests/<Kata>.Tests -f net8.0
  dotnet sln add src/<Kata>/<Kata>.csproj tests/<Kata>.Tests/<Kata>.Tests.csproj
  dotnet add tests/<Kata>.Tests/<Kata>.Tests.csproj reference src/<Kata>/<Kata>.csproj
  dotnet add tests/<Kata>.Tests/<Kata>.Tests.csproj package FluentAssertions --version 6.12.0
  rm src/<Kata>/Class1.cs tests/<Kata>.Tests/UnitTest1.cs
  ```
  Add `.gitignore` with `bin/ obj/ *.user .vs/`.

  Implement the single class or static method in `src/<Kata>/`.
  Write one `[Fact]` per scenario in `tests/<Kata>.Tests/<Kata>Tests.cs`.
  Test names use `Snake_case_full_sentence_matching_scenario_title`.
  Verify: `dotnet test` — all scenarios green.

  Write `csharp/README.md` and `csharp/WALKTHROUGH.md`. For Tier 1, the walkthrough is short: "Algorithmic kata. No domain builders — the algorithm inputs/outputs are the domain. See `src/<Kata>/` for the one function, `tests/<Kata>.Tests/` for scenario-per-test mapping."

- [ ] **3. TypeScript implementation**

  Scaffold:
  ```bash
  cd <kata>/typescript
  npm init -y
  npm pkg set type=module
  npm pkg set scripts.test="vitest run"
  npm install -D typescript vitest@^1.6.0 @types/node
  ```
  Add `tsconfig.json` (strict, ES2022 module, `src/**/*`, `tests/**/*`).
  Add `vitest.config.ts` (`test: { globals: true, include: ['tests/**/*.test.ts'] }`).
  Add `.gitignore` (`node_modules/ dist/ coverage/ *.log`).

  Implement in `src/<camelName>.ts` (or a small set of files if the algorithm naturally decomposes).
  Tests in `tests/<camelName>.test.ts`, one `it(...)` per scenario, titles matching `SCENARIOS.md` verbatim.
  Verify: `npx vitest run` — all green.

  Write `typescript/README.md` and `typescript/WALKTHROUGH.md`.

- [ ] **4. Python implementation**

  Scaffold:
  ```bash
  mkdir -p <kata>/python/src/<snake_name> <kata>/python/tests
  ```
  Create `pyproject.toml` with `[project] name="<kata>"`, `requires-python=">=3.11"`, `[project.optional-dependencies] dev=["pytest>=7.4"]`, `[tool.setuptools.packages.find] where=["src"]`, `[tool.pytest.ini_options] testpaths=["tests"] pythonpath=["src"]`.
  Add `.gitignore` (`__pycache__/ *.pyc .pytest_cache/ .venv/ *.egg-info/ dist/ build/`).

  Implement in `src/<snake_name>/<module>.py`, export from `__init__.py`.
  Tests in `tests/test_<module>.py` (plus `tests/__init__.py` empty).
  Verify:
  ```bash
  cd <kata>/python && python3 -m venv .venv && .venv/bin/pip install -q pytest && .venv/bin/pytest
  ```
  All green.

  Write `python/README.md` and `python/WALKTHROUGH.md`.

- [ ] **5. Wire astro-site page**

  Edit `astro-site/src/content/katas/<kata>.md`:
  - Frontmatter: add `solutionsCSharp/TypeScript/Python` URLs pointing to the three language folders on `https://github.com/Kode-Rex/tddbuddy-reference-katas/tree/main/<kata>/...`
  - End of body: add `## Reference Walkthrough` section matching the shape of `bank-account.md`'s section.

  Verify astro-site builds: `npx astro build`.

- [ ] **6. Update top-level README**

  Edit `README.md` Kata Status table: add a row for this kata with ✅/✅/✅ and gear `Low`.

- [ ] **7. Commit & push**

  Two commits:
  - Reference-katas: `<kata>: reference implementations across C#, TypeScript, Python` (one commit is fine at this gear for a Tier-1 kata — the implementations are small; splitting per language is optional).
  - Astro-site: `<Kata Title>: link reference solutions + walkthrough`.

  Push both.

---

### Template B — Tier 2 (Low gear, light builder)

**Difference from Template A:** add a small builder in the test project for the one primary entity. The builder is 10–30 lines. No object mothers. No value types unless the domain genuinely has one (e.g. `Hand` in tennis-score is just a pair of counts — a builder is warranted, `Score` value type is not).

**Commit granularity:** one commit per language, as with Template A. The builder ships with the tests, not as a separate commit.

Otherwise follow Template A's tasks 1–7. In Task 1, the `README.md` names the single builder. In the per-language implementation, the test folder contains `<Entity>Builder.<ext>` alongside the test file.

---

### Template C — Tier 3 (Middle gear, full builders)

**Reference implementation:** `bank-account/` (start here), then `gilded-rose/` (for category-switched domain logic).

**Difference from Template B:** multiple builders, domain value types (`Money`, `Quantity`, `SKU` as appropriate), collaborator interfaces for time/notification/external effects (`Clock`, `Notifier`). Object mothers where a named canonical instance reads clearly (`UserMother.AdultMember`).

**Tasks:**

- [ ] **1. Draft shared spec**

  `<kata>/README.md` — which builders, which collaborators, which teaching patterns showcase this kata.

  `<kata>/SCENARIOS.md` — follows the Gilded Rose / Bank Account template:
  - `## Ubiquitous Vocabulary` table
  - `## Domain Rules` section
  - `## Test Scenarios` with subsections and numbered scenarios (typically 18–25)

- [ ] **2. C# implementation**

  Scaffold as in Template A. Additionally:
  - Domain types in `src/<Kata>/` — value types as `readonly record struct` where they're pure data (e.g. `Money`, `Quantity`), classes where they have identity (e.g. `Account`, `Cart`).
  - Collaborator interfaces for external effects (`IClock`, `INotifier`).
  - In `tests/`:
    - `<Collaborator>Fake.cs` for each interface (e.g. `FixedClock`, `RecordingNotifier`).
    - `<Entity>Builder.cs` for each builder — fluent, returning tuples if the builder needs to expose multiple objects (e.g. account + clock).
  - Tests use builders + fakes for setup. Assertions use FluentAssertions.

  Verify: `dotnet test` green.

  Write `csharp/README.md` and `csharp/WALKTHROUGH.md`. Walkthrough is **design rationale** in the style of `bank-account/csharp/WALKTHROUGH.md`: why is X a type, why is Y an interface, why do the builders return tuples, etc. Not a commit tour.

- [ ] **3. TypeScript implementation**

  Mirror the C# design:
  - Classes for entities; `readonly` fields; `Protocol`-like structural interfaces for collaborators.
  - Use `Date` carefully — `new Date(Date.UTC(y, m, d))` in tests to avoid timezone drift.
  - Builders in `tests/<Entity>Builder.ts`.
  - Fakes in `tests/<Collaborator>Fake.ts`.

  Verify: `npx vitest run` green.

  Walkthrough is a **delta** from the C# walkthrough: "Same design; here's what's idiomatic to TypeScript."

- [ ] **4. Python implementation**

  Mirror the C# design:
  - `@dataclass(frozen=True)` for value types; regular classes for entities.
  - `Protocol` for collaborators.
  - `Decimal` for money.
  - Builders in `tests/<entity>_builder.py`.
  - Fakes in `tests/<collaborator>_fake.py`.
  - Re-export public surface via `src/<snake_name>/__init__.py`.

  Verify: `.venv/bin/pytest` green.

  Walkthrough is a delta from C#.

- [ ] **5. Wire astro-site page** (same as Template A Task 5)

- [ ] **6. Update top-level README** — gear `Middle` (or `Low` if the kata was driven commit-per-scenario for teaching).

- [ ] **7. Commit & push**

  Four commits per kata (one per language + one for walkthroughs/README + one astro-site):
  - `<kata>: C# reference implementation`
  - `<kata>: TypeScript reference implementation`
  - `<kata>: Python reference implementation`
  - `<kata>: walkthroughs` (and top-level README row update)
  - (astro-site) `<Kata Title>: link reference solutions + walkthrough`

  Push both repos.

---

### Template D — Tier 4 (Special)

Out of scope for this plan. Each Tier-4 kata needs its own brainstorming → plan → execution cycle. Do not dispatch subagents against Tier-4 katas with templates A/B/C — the domains genuinely differ.

---

## Subagent Dispatch Strategy

**One subagent per kata.** A subagent takes a single kata and executes the full template (all three languages + astro-site + commits + push) in isolation.

**Do not parallelize languages within a kata.** The three language implementations share the SCENARIOS.md spec and the walkthroughs cross-reference each other; sequencing is cheap, parallelism introduces merge friction.

**Do parallelize katas.** Multiple katas are genuinely independent. Batch size 3–5 subagents at a time is safe (three separate kata folders, no shared files except `README.md` and astro-site `src/content/katas/` entries, which are trivial merge-at-end).

**Review between batches.** After each batch of subagents completes:
1. Run the full test matrix in each touched kata (`dotnet test`, `npx vitest run`, `.venv/bin/pytest`) — don't trust the subagent's report.
2. Skim each WALKTHROUGH.md for accuracy.
3. Build astro-site once with `npx astro build` after each batch.

**Subagent prompt skeleton (copy per dispatch):**

```
Implement the <kata-name> reference kata across C#, TypeScript, and Python
in /Users/travisfrisinger/Documents/projects/tddbuddy/tddbuddy-reference-katas.

Spec: astro-site/src/content/katas/<kata-name>.md
Template: docs/plans/2026-04-14-remaining-katas.md § Template <A|B|C>
Tier: <1|2|3>
Reference implementations to mirror in shape and style:
  - bank-account/  (middle gear, Tier 3 if C)
  - gilded-rose/   (low gear, commit-per-scenario if you want to see that cadence)

Deliver:
  - <kata>/README.md + SCENARIOS.md
  - Three language implementations (all tests green)
  - Three WALKTHROUGH.md files
  - astro-site/src/content/katas/<kata>.md updated with solution URLs and walkthrough section
  - Row added to reference-katas README.md Kata Status table
  - Commits in both repos with the commit-message shape from § Template <A|B|C> Task 7
  - Both repos pushed to origin/main

Do not skip the verification runs. Report test counts for each language.
```

---

## Acceptance Criteria (per kata)

- [ ] `<kata>/SCENARIOS.md` enumerates every behavior from the kata spec
- [ ] Each language folder has: source, tests, README, WALKTHROUGH, scaffold files, `.gitignore`
- [ ] All three language test suites green, count matches SCENARIOS.md scenario count
- [ ] `astro-site/src/content/katas/<kata>.md` frontmatter has three solution URLs
- [ ] `astro-site/src/content/katas/<kata>.md` body has a Reference Walkthrough section
- [ ] `npx astro build` succeeds
- [ ] Top-level `README.md` Kata Status row updated with ✅/✅/✅
- [ ] Both repos pushed; HEAD advanced on origin/main

---

## Order of Execution (recommended)

Start with the remaining three **Tier 3** katas that already have `SCENARIOS.md` drafted, since they are the highest-value teaching artifacts and their specs are written:

1. `video-club-rental` — already has SCENARIOS.md
2. `shopping-cart` — already has SCENARIOS.md
3. `library-management` — already has SCENARIOS.md
4. `poker-hands` — already has SCENARIOS.md

Then Tier 3 katas without scenarios (bulk of remaining Tier 3 — 26 katas).

Then Tier 2 (13 katas). Fast dispatch — each is ~30 minutes of subagent work.

Then Tier 1 (25 katas). Fastest — each is ~15 minutes.

Tier 4 (3 katas) gets its own plan later.

---

## Self-Review Notes

- **Spec coverage:** every kata in `astro-site/src/content/katas/` (minus gilded-rose + bank-account + tier-4 trio) is in the Classification Table above.
- **Placeholder scan:** no TODO/TBD/placeholder text in the templates — every step either shows the command or references a concrete reference implementation.
- **Type consistency:** templates reference entities generically (`<Kata>`, `<Entity>`, `<Collaborator>`); per-kata subagents resolve these from the spec.
