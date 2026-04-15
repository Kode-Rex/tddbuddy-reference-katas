# Character Copy

Copy characters one at a time from a `Source` to a `Destination`, stopping when the source produces a newline (`'\n'`). The newline itself is *not* written to the destination.

This kata ships in **Agent Full-Bake** mode at the F1 tier. It is the one F1 kata whose teaching point is not a pure function — the SUT is a `copy(source, destination)` procedure that coordinates two **collaborator interfaces**. It is Kent Beck's classic "mocking" kata: you cannot drive it without test doubles because the SUT's entire behaviour is the sequence of calls it makes.

## Collaborators

Two tiny interfaces live in `src/`:

- **`Source`** — `readChar() → char`. Hands the copier one character at a time.
- **`Destination`** — `writeChar(c)`. Receives one character at a time.

Two hand-rolled fakes live in `tests/` (not `src/`):

- **`StringSource`** — wraps a string; each `readChar()` returns the next character, appending `'\n'` as the terminator when the wrapped string is exhausted.
- **`RecordingDestination`** — collects every character written into a string the test can assert against.

This is the F1 shape for the "mocking" kata: **collaborator interfaces + hand-rolled fakes**, no builders, no mocking framework. Tests assert on the recorded state of the fake destination, not on call sequences — the behaviour is "what was written", not "how many times was it called".

The original TDD Buddy brief mentions a mocking *framework*; the reference deliberately uses hand-rolled fakes instead. A two-method interface with a five-line fake is clearer than any framework setup, and the test reads as a sentence about the domain. See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification, and each language's `WALKTHROUGH.md` for the design note.

The **bonus** (`readChars(n)` / `writeChars(chars)` batch operations) is intentionally out of scope — the single-character streaming loop is sufficient to demonstrate the collaborator-plus-fake pattern.
