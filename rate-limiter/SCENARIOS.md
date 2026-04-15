# Rate Limiter — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Limiter** | The aggregate (`Limiter`). Owns the rule and the per-key window state |
| **Rule** | Value type: `maxRequests` (strictly positive count) + `window` (strictly positive duration) |
| **Key** | A client identifier string (e.g. `"alice"`); each key has an independent window and count |
| **Window** | The fixed-length interval `[windowStart, windowStart + rule.window)` during which the count accumulates |
| **Decision** | Result of `request(key, now)` — either `Allowed` or `Rejected(retryAfter)` |
| **RetryAfter** | Duration returned on `Rejected` — how long until the current window ends (i.e., when the next request would be allowed) |
| **Clock** | Collaborator returning "now" — injected so window-expiry tests control time without sleeping |

## Domain Rules

- A **Rule** must have `maxRequests >= 1` and `window > 0`. Non-positive values for either are rejected with `LimiterRuleInvalidException` and a byte-identical message per field.
- A new `Limiter` has no recorded windows; every key is effectively at count zero.
- `request(key, now)` is the only mutating operation. It returns a `Decision`.
- The **first request** for a key opens a window at `windowStart = now` with `count = 1` and returns `Allowed`.
- Subsequent requests **within the same window** (`now < windowStart + rule.window`) increment the count if `count < maxRequests` and return `Allowed`.
- A request arriving when `count == maxRequests` **within the same window** returns `Rejected(retryAfter)` where `retryAfter = (windowStart + rule.window) - now`. The count does **not** change.
- A request arriving when `now >= windowStart + rule.window` **opens a new window** at `windowStart = now` with `count = 1` and returns `Allowed`. The previous window is discarded.
- **Keys are isolated.** A rejection on `"alice"` does not affect `"bob"`; each key has its own `(windowStart, count)`.
- The clock is **read once per call** and every comparison in that call uses the same `now`.

## Named Constants

- `DefaultMaxRequests = 100`
- `DefaultWindowDuration = 60 seconds` (`TimeSpan.FromSeconds(60)` / `60_000 ms` / `timedelta(seconds=60)`)

These defaults reflect a common production-shape limit ("100 req/min per client"). The kata's worked example uses `3 req / 10 s`; builders override both per scenario.

## Test Scenarios

### Rule Construction

1. **A rule with positive maxRequests and positive window is valid**
2. **A rule rejects zero maxRequests with LimiterRuleInvalidException**
3. **A rule rejects negative maxRequests with LimiterRuleInvalidException**
4. **A rule rejects zero window with LimiterRuleInvalidException**
5. **A rule rejects negative window with LimiterRuleInvalidException**

### Allowed Requests Under the Limit

6. **The first request for a key is allowed**
7. **Requests up to the limit within the window are all allowed**
8. **Each Allowed decision carries no retryAfter**

### Rejection at the Limit

9. **The request past the limit within the window is rejected**
10. **A rejection reports retryAfter as the remaining window duration**
11. **A rejected request does not count against the window (count stays at max)**
12. **Repeated rejections all report a decreasing retryAfter as the clock advances**

### Window Expiry Resets Count

13. **A request exactly at the window boundary opens a fresh window and is allowed**
14. **A request after the window has fully elapsed opens a fresh window and is allowed**
15. **After a window resets, the full quota is available again**

### Multiple Keys Are Isolated

16. **Two different keys have independent counts**
17. **A rejection on one key does not affect another key's decisions**
18. **Each key's window starts from its own first request, not a shared global clock**

### End-to-End Worked Example

19. **A fixed-window cycle (3 req / 10 s): allow t=0,1,2 for alice; reject t=3 with retryAfter=7s; allow t=3 for bob; at t=10 alice's window resets and three more requests are allowed before a fourth is rejected**
