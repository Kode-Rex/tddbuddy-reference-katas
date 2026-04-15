# IP Validator

Validate whether a string is a **host-assignable IPv4 address** in dotted-decimal notation.

This kata ships in **Agent Full-Bake** mode at the F1 tier: an algorithmic kata with **no builders**. The input is a `string` and the output is a `bool` — the inputs and outputs *are* the domain, so there are no aggregates to construct, no value types to introduce, and no collaborators to inject. The teaching point is that scenario-as-test naming still carries when the domain is this thin: each test reads as one line from the spec table.

IPv6 is **out of scope** — the TDD Buddy spec is IPv4-only.

**No regular expressions.** Per the TDD Buddy prompt, parsing is done by splitting on `.` and validating each octet by hand. The walkthrough notes each language's idiomatic split+parse pair.

**Spec conflict note.** The astro-site kata page lists `192.168.1.0 → true` in its example table, which contradicts the prose rule ("any address ending in 0 or 255 is not a valid host address") and the `0.0.0.0 → false` / `255.255.255.255 → false` rows. The prose rule is taken as authoritative here: any address whose final octet is `0` or `255` is rejected as a non-host address. This matches the kata's well-known intent of rejecting network and broadcast addresses for host assignment.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
