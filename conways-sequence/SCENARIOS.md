# Conway's Sequence — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Domain Rules

- **Digit run** — a maximal run of one identical digit in a term.
- **Next term** — for each run in left-to-right order, emit the run length followed by the digit. `"111221"` has runs `111`, `22`, `1` → `"312211"`.
- **Look-and-say(seed, n)** — apply next-term `n` times to `seed`. `n = 0` returns the seed unchanged. `n` must be non-negative.
- **Run lengths above nine** are emitted as their decimal representation (e.g. ten consecutive `1`s become `"101"`). The spec flags this as an edge case; the reference follows the "describe as decimal" convention.
- **Seeds may contain any decimal digits**, not only `"1"`. The algorithm never inspects digit identity beyond equality.

## Test Scenarios

### `next_term`

1. **`"1"` has one 1** → `"11"`
2. **`"11"` has two 1s** → `"21"`
3. **`"21"` has one 2 and one 1** → `"1211"`
4. **`"1211"` has one 1, one 2, two 1s** → `"111221"`
5. **`"111221"` has three 1s, two 2s, one 1** → `"312211"`
6. **`"2"` alone is one 2** → `"12"`
7. **`"22"` is two 2s** → `"22"` (a fixed point)
8. **`"3211"` has one 3, one 2, two 1s** → `"131221"`
9. **Ten consecutive 1s are described as ten 1s** → `"101"`

### `look_and_say(seed, n)`

10. **Zero iterations returns the seed unchanged** — `look_and_say("1", 0)` → `"1"`
11. **One iteration equals a single `next_term`** — `look_and_say("1", 1)` → `"11"`
12. **Five iterations from `"1"` land on `"312211"`** — `look_and_say("1", 5)` → `"312211"`
13. **Two iterations from seed `"2"`** — `look_and_say("2", 2)` → `"1112"`
14. **Negative iteration count is rejected** — raises the language-idiomatic argument exception.
