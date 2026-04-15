# Fizz Buzz Whiz

The classic FizzBuzz: a pure function `int → string` that substitutes `"Fizz"` for multiples of 3, `"Buzz"` for multiples of 5, `"FizzBuzz"` for multiples of both, and the number itself otherwise.

This kata ships in **Agent Full-Bake** mode at the F1 tier: an algorithmic kata with **no builders**. The input is an `int` and the output is a `string` — the inputs and outputs *are* the domain, so there are no aggregates to construct, no value types to introduce, and no collaborators to inject. The teaching point is that scenario-as-test naming still carries when the domain is this thin: each test reads as one line from the spec table.

The TDD Buddy prompt includes a "Whiz" bonus (return `"Whiz"` for primes). **That bonus is out of scope for this reference implementation** — FizzBuzz alone is sufficient to demonstrate the F1 shape. See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
