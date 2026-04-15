# Prime Factors — Scenarios

The shared specification all three language implementations satisfy. The order **is** the curriculum — each scenario is where it is because the previous scenarios made it reachable, and the later scenarios are proof-by-example that the algorithm generalized.

## Domain Rules

- `generate(n: int) -> list<int>` takes a positive integer and returns its prime factors in ascending order.
- If a prime factor appears more than once, it appears that many times in the output.
- `generate(1)` returns the empty list.

## Scenarios

1. **`1` has no prime factors.** `generate(1) == []`.
2. **`2` is its own only prime factor.** `generate(2) == [2]`.
3. **`3` is its own only prime factor.** `generate(3) == [3]`.
4. **`4` factors into two twos.** `generate(4) == [2, 2]`.
5. **`6` factors into two and three.** `generate(6) == [2, 3]`.
6. **`8` factors into three twos.** `generate(8) == [2, 2, 2]`.
7. **`9` factors into two threes.** `generate(9) == [3, 3]`.
8. **`12` factors into two, two, three.** `generate(12) == [2, 2, 3]`.
9. **`15` factors into three and five.** `generate(15) == [3, 5]`.
10. **`100` factors into two, two, five, five.** `generate(100) == [2, 2, 5, 5]`.
11. **`30030` factors into the first six primes.** `generate(30030) == [2, 3, 5, 7, 11, 13]`.

## Out of Scope

Behavior not listed above is undefined by this spec — `0`, negative inputs, non-integer inputs. No tests are written for those. If a future requirement names them, a scenario is added here first.
