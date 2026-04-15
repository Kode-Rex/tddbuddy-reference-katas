# URL Shortener

Maintain an in-memory bidirectional map between long URLs and short codes, issue short URLs of the form `https://short.url/<code>`, translate in either direction, and track per-short-URL visit counts that drive a `statistics` view.

This kata ships in **Agent Full-Bake** mode at the F1 tier: a small stateful `Shortener` class with no builders. The state — long-to-short map, short-to-long map, and per-code visit counter — is trivial enough that a fresh `Shortener` instance per test is all the test-side construction the scenarios need. The teaching point is that "F1" is about *builder weight*, not *statelessness*: a stateful SUT stays F1 as long as a bare constructor plus direct method calls reads cleanly.

**Short-code scheme:** base36 sequential counter. The first URL shortens to `.../0`, the second to `.../1`, the tenth to `.../a`, the thirty-seventh to `.../10`. Deterministic, collision-free by construction, and the tests can name the expected code directly without mocking a hash. A hash-based scheme would force every test to pre-compute `MD5(url)[:7]` in its head; sequential base36 makes the scenarios read like a spec.

**Covered behaviors** (from the TDD Buddy spec):

- `shorten(longUrl)` returns `https://short.url/<code>`; calling it again with the same long URL returns the existing short URL (deduplication).
- `translate(url)` accepts either the long URL or the short URL and returns the short URL. Translating via the short URL counts as a visit; translating via the long URL does not.
- `statistics(url)` accepts either form and returns the short URL, the long URL, and the visit count.
- Unknown URLs raise a language-idiomatic error (`ArgumentException` / `Error` / `ValueError`) with a byte-identical message.

The **bonus** items (invalid-URL rejection, access-time history / `log` method) are out of scope for this reference — the core spec is what the three implementations satisfy identically.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
