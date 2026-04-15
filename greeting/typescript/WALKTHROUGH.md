# Greeting — TypeScript Walkthrough

This is an **algorithmic string-formatting kata**: the input is a name, `null`, or an array of names, the output is a string, and there are no aggregates, value types, or collaborators — the inputs and outputs *are* the domain. The reference lands as a single commit: `src/greeting.ts` exports `greet(name: string | null | ReadonlyArray<string | null>): string`. The single union-typed entry handles the single/null/list cases in one signature (TS's union types make this cleaner than C#'s overload pair). Internally the function resolves `null` to `"my friend"`, partitions the names into normals and shouts, joins each partition with its own shape rule, and — when both partitions are non-empty — concatenates them with `". AND "`. `tests/greeting.test.ts` has one `it()` per scenario in [`../SCENARIOS.md`](../SCENARIOS.md); each test name reads as a sentence from that spec.

**Shout detection.** A shout is a string that contains at least one letter and whose uppercase form equals itself: `/[A-Za-z]/.test(n) && n === n.toUpperCase()`. Names without letters (pure punctuation) are treated as normals, avoiding a spurious `"HELLO !"`.

**Punctuation asymmetry — deliberate and spec-faithful.** The TDD Buddy prompt ends scenarios 1–2 with a trailing period but scenarios 4–5 without one; the reference honors those literals exactly rather than "fixing" the inconsistency.

**Inline literals — deliberate.** The strings `"Hello, "`, `"my friend"`, `". AND "`, and `"HELLO "` stay inline. F1 katas treat the literal rules as the rule itself; naming them as constants would write the phrase twice.

The two bonus tasks from the TDD Buddy prompt are intentionally out of scope — see the kata [`README.md`](../README.md).
