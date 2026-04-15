# Prime Factors — Python Walkthrough

This kata ships in **[Pedagogy mode](../../README.md#1-pedagogy--learn-the-tdd-rhythm)**. The commit log is the teaching. Read it end to end to feel the rhythm — red → green, red → green, refactor, reflect — and the gear shifts from **low** (fake-it, one data point at a time) to **middle** (obvious implementation) to **high** (later scenarios pass on arrival).

Python idioms replace the C# shape directly: `list[int]` return type, `factors.append(...)`, `n //= divisor` for integer division. The signature move — a **run of spec-on-arrival commits** after the algorithm clicks — looks the same in every language.

| Step | Commit | Cycle | Gear | What was learned |
|------|--------|-------|------|------------------|
| 1 | [`20e103d`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/20e103d) | scaffold | — | pytest + `src/` layout, pyproject. No SUT yet. |
| 2 | [`e2dfd3d`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/e2dfd3d) | red | low | Import of `prime_factors.factors` fails at collection — that's the red. |
| 3 | [`40af124`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/40af124) | green (fake-it) | low | `return []`. Restraint is the lesson. |
| 4 | [`0bffe79`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/0bffe79) | red | low | `generate(2) == [2]`. |
| 5 | [`9694435`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/9694435) | green | low | `if n > 1: return [n]` — still mostly fake. |
| 6 | [`ef06319`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/ef06319) | spec (passes on arrival) | low | `generate(3) == [3]` — honest pass. |
| 7 | [`7041486`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/7041486) | red | low | `generate(4) == [2, 2]` — first composite. |
| 8 | [`bf42462`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/bf42462) | green | low | Divide-out-2 loop with `n //= 2`. |
| 9 | [`3ded605`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/3ded605) | reflect | low | Empty commit. A general loop is peeking through. |
| 10 | [`bc51fcf`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/bc51fcf) | spec (passes on arrival) | low | `generate(6) == [2, 3]` — honest pass. |
| 11 | [`0a27b15`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/0a27b15) | spec (passes on arrival) | low | `generate(8) == [2, 2, 2]` — honest pass. |
| 12 | [`9471f6b`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/9471f6b) | red | low → middle | `generate(9) == [3, 3]` — **the hinge.** First odd composite. |
| 13 | [`2173c4d`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/2173c4d) | green | middle | Outer `while n > 1` / inner `while n % divisor == 0`. **Algorithm complete.** |
| 14 | [`c12163d`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/c12163d) | reflect | middle | Empty commit. Proof-by-example begins. |
| 15 | [`cc6e600`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/cc6e600) | spec (passes on arrival) | middle | `generate(12) == [2, 2, 3]`. No new code. |
| 16 | [`af808d5`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/af808d5) | spec (passes on arrival) | middle | `generate(15) == [3, 5]`. No new code. |
| 17 | [`7cf08f1`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/7cf08f1) | spec (passes on arrival) | high | `generate(100) == [2, 2, 5, 5]`. No new code. |
| 18 | [`c37a5b0`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/c37a5b0) | spec (passes on arrival) | high | `generate(30030) == [2, 3, 5, 7, 11, 13]`. Six distinct primes, no new code. |

## How to run

```bash
cd prime-factors/python
python3 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```

## The takeaway

Seven spec-on-arrival commits in the chain. The Python idiom for integer division (`//=`) makes the divide step read exactly as the algorithm narrates: "divide `n` by `divisor` in place." No surprise with `float` creeping in, no cast noise. That clarity is why the imperative shape wins over a `while True: yield ...` generator here — the spec talks about a finite list, and the code answers with a finite list.

Divergence from the other languages: Python's `list[int]` return type lands without parentheses or generics noise; `list[int] = []` typed annotation on the local is the most explicit line in the function. The rest is the algorithm.
