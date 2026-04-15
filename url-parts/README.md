# URL Parts

Decompose a URL string into its component parts: **protocol**, **subdomain**, **domain** (host + top-level domain), **port** (defaulted from protocol when absent), **path** (with leading `/` stripped), **parameters** (the query string after `?`), and **anchor** (the fragment after `#`).

This kata ships in **Agent Full-Bake** mode at the F1 tier: an algorithmic kata with **no builders**. The input is a `string` and the output is a typed record with seven string/int fields — the inputs and outputs *are* the domain, so there are no aggregates to construct, no value types beyond the result shape, and no collaborators to inject. The teaching point is that a typed return record keeps the tests readable (`parts.Subdomain.Should().Be("www")`) without tipping into builder territory: the record is the data shape, not a fluent construction API.

**Do not use built-in URL classes.** Per the TDD Buddy prompt, parsing is done by hand — no `System.Uri`, no `new URL(...)`, no `urllib.parse`. The walkthroughs note each language's idiomatic `split`/`indexOf` pair and why hand-rolling keeps the parse rules visible in one screen.

**Scope limits** (from the spec):

- Only the four listed protocols: `http` (80), `https` (443), `ftp` (21), `sftp` (22).
- Only top-level TLDs from the fixed list (`.com`, `.net`, `.org`, `.int`, `.edu`, `.gov`, `.mil`). Second-level TLDs like `.co.uk` are out of scope.
- `localhost` has no TLD and no subdomain — it is a bare host.
- Path loses its leading `/`; query loses its leading `?`; anchor loses its leading `#`.

See [`SCENARIOS.md`](SCENARIOS.md) for the shared specification.
