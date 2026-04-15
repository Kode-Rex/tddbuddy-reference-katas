# Greeting — C# Walkthrough

This is an **algorithmic string-formatting kata**: the input is a name or a list of names, the output is a string, and there are no aggregates, value types, or collaborators — the inputs and outputs *are* the domain. The reference lands as a single commit: `src/Greeting/Greeting.cs` exposes a static `Greeter.Greet(...)` with two overloads (one for `string?`, one for `string?[]`) so callers don't have to wrap a single name in an array at the boundary. Internally both overloads funnel through a private `GreetMany` that partitions the input into normals and shouts, joins each partition with its own shape rule, and — when both partitions are non-empty — concatenates them with `". AND "`. `tests/Greeting.Tests/GreeterTests.cs` has one `[Fact]` per scenario in [`../SCENARIOS.md`](../SCENARIOS.md); each test name reads as a sentence from that spec.

**Punctuation asymmetry — deliberate and spec-faithful.** The TDD Buddy prompt ends scenarios 1–2 with a trailing period but scenarios 4–5 without one; the reference honors those literals exactly rather than "fixing" the inconsistency. A `formatOptions` parameter here would be gold-plating a six-scenario kata.

**Inline literals — deliberate.** The strings `"Hello, "`, `"my friend"`, `". AND "`, and `"HELLO "` are kept inline rather than extracted as named constants. In F1 katas the function body fits on one screen and the literals *are* the rule — naming `StandIn = "my friend"` would write the phrase twice without adding meaning. F3 and larger modes prefer named constants; F1 deliberately doesn't.

The two bonus tasks from the TDD Buddy prompt (splitting comma-separated entries, escaping commas with quoted entries) are intentionally out of scope — see the kata [`README.md`](../README.md).
