# Gilded Rose — Reference Implementation

The Gilded Rose is the canonical refactoring kata. A small inventory system with convoluted rules and a `updateQuality` method that nobody wants to touch. The exercise is to refactor it into something that accommodates a new item category without rewriting everything.

This reference implementation showcases:

- **Test Data Builders** — items built fluently: `anItem().named("Aged Brie").withQuality(10).withSellIn(5)`
- **Object Mothers** — canonical categories: `aLegendaryItem()`, `anAgedItem()`, `aBackstagePass()`, `aConjuredItem()`
- **Domain Types over primitives** — `Quality`, `SellIn` — not `int`
- **Scenario-named tests** — `Quality_of_aged_brie_increases_as_it_ages`, not `TestAgedBrie_1`
- **Ubiquitous language** — "legendary", "conjured", "backstage pass", "quality", "sell-in" — the vocabulary from the kata brief flows into tests, builders, and domain types without translation
- **Polymorphism emerging from tests** — the final design has item categories as first-class types, not string tags, *because the tests drove that distinction*

## Why This Kata Earns the Treatment

Gilded Rose is the classic case for showing that good TDD produces specification-grade tests. The original code is a `switch`-heavy mess of if-statements keyed on item name strings. A naive TDD attempt produces tests like `TestUpdateQuality_AgedBrie_IncreasesBy1`. That's a test name, but it isn't a specification.

The reference implementation uses the kata to demonstrate what the three-post arc on the [TDD Buddy blog](https://www.tddbuddy.com/blog) argues: tests should read as behavior, setup should speak domain, and the types should carry the vocabulary of the business.

## Layout

- [`SCENARIOS.md`](SCENARIOS.md) — the shared specification all three languages satisfy
- [`csharp/`](csharp/) — C# implementation with xUnit + FluentAssertions
- [`typescript/`](typescript/) — TypeScript implementation with Vitest
- [`python/`](python/) — Python implementation with pytest

Each language folder has its own README (how to build and run) and WALKTHROUGH (commit-by-commit teaching tour).

## How to Read This Kata

1. Read [`SCENARIOS.md`](SCENARIOS.md) first — it's the spec.
2. Pick your preferred language and read its `WALKTHROUGH.md` to follow the TDD progression.
3. Check out the commits referenced in the walkthrough to see the code at each step.
4. Read the final code for the full picture.

## Related

- Blog: [TDD Already Does BDD — Without the Gherkin](https://www.tddbuddy.com/blog/tdd-already-does-bdd/)
- Reference: [Test Data Builders](https://www.tddbuddy.com/references/test-data-builders)
- Reference: [Ubiquitous Language in Tests](https://www.tddbuddy.com/references/ubiquitous-language-in-tests)
- Kata page: [Gilded Rose on TDD Buddy](https://www.tddbuddy.com/katas/gilded-rose)
