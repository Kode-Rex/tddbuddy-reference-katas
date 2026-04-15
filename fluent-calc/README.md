# Fluent Calc

A calculator with a fluent, chained API: `new Calculator().Seed(10).Plus(5).Minus(2).Result()`. Supports `Undo` and `Redo` as part of the chain, and a `Save` that drops history so subsequent `Undo`/`Redo` have no effect.

This kata ships in **Agent Full-Bake** mode at the F1 tier: the fluent API **is** the SUT. Tests drive scenarios by chaining the calculator's own methods, so no test data builder earns its keep — the chain itself is the builder. The F1 rule "inputs and outputs are the domain" bends slightly (a `Calculator` is a stateful object, not a pure function), but the F1 discipline holds: no auxiliary builders, no domain value types beyond `int`, no collaborators.

Per the TDD Buddy prompt, the calculator **never throws** — operations on an unseeded calculator, or `Undo`/`Redo` with no history, return the chain untouched. After the first `Seed`, additional `Seed` calls are ignored. After `Save`, history is cleared so `Undo` and `Redo` have nothing to replay.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification that all three language implementations satisfy.
