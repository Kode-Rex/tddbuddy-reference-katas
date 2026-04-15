# Password — C# Walkthrough

This kata ships in **middle gear** — the whole C# implementation landed in one commit once the design was understood. This walkthrough explains **why the design came out the shape it did**, not how the commits unfolded.

It is also the first **F2** reference: one primary entity (`Policy`), one small test-folder builder (`PolicyBuilder`), and nothing else. Read the repo [Gears section](../../README.md#gears--bridging-tdd-and-bdd) for why middle gear is the deliberate choice, not a corner cut.

## Scope — Policy Only

The original TDD Buddy Password prompt describes a credential store, a mocked password repository, an email service, reset tokens, and one-hour token expiry. **All of that is deliberately out of scope here.** Credential storage, repository collaborators, email services, token generators, and password history are F3-scale responsibilities — each introduces its own interface, its own fake, its own set of mocks-as-specifications. Shipping them here would dilute the F2 teaching point, which is narrower and more focused: *the builder payoff on one primary entity.*

See the kata [`README.md`](../README.md#stretch-goals-not-implemented-here) for the full stretch-goal list.

## The Design at a Glance

```
Policy (record) ──Validate──> ValidationResult (record)
  MinLength                     Ok: bool
  RequiresDigit                 Failures: IReadOnlyList<string>
  RequiresSymbol
  RequiresUpper
  RequiresLower

PolicyBuilder (tests/) ──Build──> Policy
```

Three files under `src/Password/` (the entity, the result, the rule-name and default-length constants) and one builder under `tests/Password.Tests/`. That is the whole F2 surface.

## Why `Policy` Is a Record

`Policy` has no identity — two policies with the same rules *are* the same policy. That is textbook value-equality: `record` gets it for free, along with immutability and a useful `ToString()` for test failure diagnostics. Making it a `class` would force us to write `Equals` and `GetHashCode` ourselves, and every test comparing one policy to another would either work by reference accident or require custom equality. `record` says "this is a value" and the compiler does the rest.

The same reasoning applies to `ValidationResult`: a result is a pair of data (a bool and a list of failure names), not a thing with identity. Record.

## Why `ValidationResult` Returns a List of Failures (Not Just a Bool)

The simpler alternative is `bool IsSatisfiedBy(password)`. It is tempting. It is also wrong for an F2 kata.

**The failures *are* the spec.** Scenario 2 in [`../SCENARIOS.md`](../SCENARIOS.md) does not say "rejects a 6-character password"; it says "rejects a 6-character password — `failures` contains `minimum length`." Scenario 9 asserts a specific failure ordering when four rules fail at once. A bool-only API hides all of that — a test could only assert pass/fail, and every real caller (a web form, an API response, a CLI) would need a second mechanism to explain *which rule* failed.

Returning the list of failure names puts the spec *in* the return value. Tests assert on the exact rule names (`"minimum length"`, `"requires digit"`, etc.) — and those strings are identical byte-for-byte across C#, TypeScript, and Python, codified in `RuleNames`. If the spec changes, the test changes; if a rule is renamed, every language must track together.

The `Ok` bool is derived: `failures.Count == 0`. It is kept on the result for reading convenience, not as a second source of truth.

## Why `PolicyBuilder` Exists — The F2 Signature Pattern

Ten scenarios need ten slightly different policies. Without a builder, each test opens with something like:

```csharp
var policy = new Policy(minLength: 8, requiresDigit: true, requiresSymbol: false, requiresUpper: false, requiresLower: false);
```

That line repeats four false-valued flags that the scenario does not care about. The *variation* — "requires a digit" — is buried behind three positional booleans. Every test author has to read every argument to find the one that matters.

With `PolicyBuilder`, the same setup reads:

```csharp
var policy = new PolicyBuilder().RequiresDigit().Build();
```

One line, one verb, one variation named. The builder ships a sensible default (`MinLength=8`, every requirement off) so tests opt *into* rules rather than opt out of them. That default is codified as `PolicyDefaults.DefaultMinLength` — named, because this is F2 (Full-Bake mode), and named constants win when a business number appears in multiple places.

Eighteen lines of builder, maybe twenty with braces. That is the F2 budget and this builder spends it well. No object mother, no tuple return, no collaborator injection — if the builder needed any of those, the kata would not be F2.

## What Is Deliberately Not Modeled

- **No `IPasswordRepository`** — there is no credential store here, so there is nothing to fake.
- **No `IEmailService`** — there is no reset flow here, so there is no email collaborator.
- **No `IClock`** — there is no token expiry, so no time collaboration.
- **No domain-specific exceptions** — the policy does not throw; it reports. `ValidationResult` is the rejection mechanism.
- **No password history** — the policy looks at one candidate against one ruleset; it has no memory.

Every omission above points at an F3 extension. See [`../README.md`](../README.md#stretch-goals-not-implemented-here).

## Scenario Map

The ten scenarios in [`../SCENARIOS.md`](../SCENARIOS.md) live in `tests/Password.Tests/PolicyTests.cs`, one `[Fact]` per scenario, with test names matching the scenario titles verbatim (modulo C# underscore convention).

## How to Run

```bash
cd password/csharp
dotnet test
```
