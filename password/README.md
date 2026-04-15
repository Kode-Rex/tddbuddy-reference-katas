# Password

A configurable password **policy** validates candidate passwords against a set of rules: minimum length, required character classes (digit, symbol, uppercase, lowercase). `Policy.Validate(password)` returns a `ValidationResult` naming the rules that were not satisfied — the failures *are* the spec.

This kata ships in **Agent Full-Bake** mode at the **F2 tier**: the first reference for the "light builder" template. One primary entity (`Policy`), one small test-folder builder (`PolicyBuilder`, 10–30 lines), no collaborators, no object mothers, no value types beyond primitives. The teaching point is the builder payoff: without it, every test spells out a full policy constructor call with four or five boolean arguments and a magic number; with it, each test opens with one readable line that names the variation it cares about — `new PolicyBuilder().MinLength(10).RequiresDigit().Build()`. That shift is the whole F2 reason for existing.

## Scope — Policy Only

The original TDD Buddy prompt on the astro-site describes a much larger system: a credential store with salted/hashed passwords, a mocked repository, and an email reset flow with a one-hour token expiry. **That is explicitly out of scope for this reference.** Those responsibilities introduce collaborator interfaces (`IPasswordRepository`, `IEmailService`, `IClock`, token generation, mocks-as-specifications) — which is F3 territory, not F2.

This reference is scoped to **password policy validation only**: given a candidate password and a policy, does the password satisfy the rules, and if not, which rules failed.

### Stretch Goals (Not Implemented Here)

These are deliberately left for an F3 follow-up:

- **Credential store** — `AreValidUserCredentials(userName, password)` with salted/hashed storage and a repository collaborator
- **Password reset email** — `SendResetEmail(emailAddress)` with an injected email service and a randomly generated token
- **Token expiry** — the one-hour window, plus the bonus rule that a new request extends all prior unexpired tokens to match the latest expiry
- **Password expiry** — passwords expire every 60 days
- **Password history** — the user may not reuse any of their previous five passwords
- **Integration tests** — real repository against an in-memory / test DB with migrations

Each of those belongs alongside a policy, but each introduces its own collaborators, and together they tip the kata into F3. See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification this reference does satisfy.
