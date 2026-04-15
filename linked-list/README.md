# Linked List

A singly linked list from scratch — no built-in list or array types backing the storage. Exposes the usual `append` / `prepend` / `get` / `remove` / `insertAt` / `contains` / `indexOf` / `size` surface, plus a `toArray` helper so tests can assert on the visible state.

This kata ships in **Agent Full-Bake** mode at the F1 tier: an algorithmic kata with **no builders**. The F1 rule "the algorithm's inputs and outputs *are* the domain" bends slightly here — a linked list is a data structure with identity and mutable state, not a pure function. What doesn't bend is the F1 discipline: no test data builders, no domain value types beyond the list itself. Tests construct scenarios by calling the list's own operations; the list *is* the SUT.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification that all three language implementations satisfy.
