# Password — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Scope

This specification covers **password policy validation only**. Credential storage, email reset, token expiry, and password history are **out of scope** — see the top-level [`README.md`](README.md#scope--policy-only) for the full stretch-goal list.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Policy** | An immutable set of rules a candidate password must satisfy: a minimum length plus per-character-class flags (`requiresDigit`, `requiresSymbol`, `requiresUpper`, `requiresLower`). Exposes `validate(password)`. |
| **ValidationResult** | The outcome of validating a password against a policy. Has `ok` (true when every rule passed) and `failures` (an ordered list of rule-name strings naming each rule the password failed). An `ok` result has an empty `failures` list. |
| **PolicyBuilder** | Test-folder fluent builder that produces a `Policy`. Defaults to the minimum sensible policy (`minLength=8`, no character-class requirements); chained methods opt into requirements. Exists so tests read one line of setup per scenario. |
| **Rule name** | The string embedded in a `ValidationResult.failures` entry when a rule does not pass. Exactly five strings across the kata: `"minimum length"`, `"requires digit"`, `"requires symbol"`, `"requires uppercase"`, `"requires lowercase"`. Identical byte-for-byte across all three languages — the failure messages *are* the spec. |

## Domain Rules

- A **digit** is a character in `0-9`.
- A **symbol** is any character that is not a letter and not a digit (punctuation, whitespace, etc. all count).
- An **uppercase** character is in `A-Z`; a **lowercase** character is in `a-z`.
- Evaluation order: the policy checks every enabled rule and collects all failures. A password that fails three rules produces a `ValidationResult` whose `failures` list names all three, in the canonical order: `minimum length`, `requires digit`, `requires symbol`, `requires uppercase`, `requires lowercase`.
- Default policy (from `PolicyBuilder` with no chained calls) has `minLength = 8` and no character-class requirements.

## Test Scenarios

1. **Policy with minLength 8 accepts an 8-character password** — `"abcd1234"` passes when only the length rule is active.
2. **Policy with minLength 8 rejects a 6-character password** — `"abc123"` fails; `failures` contains `"minimum length"`.
3. **Policy requiring a digit accepts a password with a digit** — `"password1"` passes.
4. **Policy requiring a digit rejects a password with no digit** — `"password"` fails; `failures` contains `"requires digit"`.
5. **Policy requiring a symbol accepts a password with a symbol** — `"password!"` passes.
6. **Policy requiring a symbol rejects a password with no symbol** — `"password1"` fails; `failures` contains `"requires symbol"`.
7. **Policy requiring uppercase rejects an all-lowercase password** — `"password1"` fails; `failures` contains `"requires uppercase"`.
8. **Policy requiring lowercase rejects an all-uppercase password** — `"PASSWORD1"` fails; `failures` contains `"requires lowercase"`.
9. **Policy with multiple requirements reports every failed rule** — a policy requiring length, digit, symbol, upper, and lower accepts `"Abcd123!"` and, against `"abc"`, reports failures in canonical order: `["minimum length", "requires digit", "requires symbol", "requires uppercase"]`.
10. **PolicyBuilder default is minLength 8 with no character-class requirements** — the default policy accepts `"abcdefgh"` (exactly 8 chars, no digits, no symbols, no uppercase) and rejects `"abcdefg"` with only `"minimum length"` in `failures`.
