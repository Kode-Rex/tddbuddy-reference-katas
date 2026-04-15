# Circuit Breaker — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Breaker** | The aggregate (`Breaker`). Wraps a fallible operation; tracks state and consecutive failures |
| **Operation** | A callable passed to `execute`; the breaker invokes it when closed or probing, and observes success or exception |
| **Closed** | Normal state: the breaker calls the operation and counts consecutive failures |
| **Open** | Tripped state: the breaker rejects calls immediately with `CircuitOpenException` and never invokes the operation |
| **HalfOpen** | Probing state after the reset timeout elapses: the next `execute` is allowed through as a single probe |
| **Failure Threshold** | Consecutive failures in `Closed` that trip the breaker `Closed → Open` |
| **Reset Timeout** | Duration after opening before the breaker transitions `Open → HalfOpen` on the next call |
| **Clock** | Collaborator returning "now" — injected so transition timing tests control time without sleeping |

## Domain Rules

- A new breaker is in the **`Closed`** state with zero consecutive failures.
- **Failure threshold** must be **strictly positive**; zero and negative values are rejected.
- **Reset timeout** must be **strictly positive**; zero and negative values are rejected.
- In `Closed`, `execute(operation)` invokes the operation and returns its result. If the operation throws, the breaker records a failure and re-throws the original exception.
- In `Closed`, a successful `execute` resets the consecutive-failure counter to zero.
- In `Closed`, the `N`-th consecutive failure (where `N == failureThreshold`) transitions the breaker to `Open` and records the time of opening.
- In `Open`, `execute(operation)` does **not** invoke the operation. It throws `CircuitOpenException` with the message `"Circuit is open"` and the state stays `Open`.
- In `Open`, once the elapsed time since opening is **greater than or equal to** the reset timeout, the next `execute` transitions the breaker to `HalfOpen` and invokes the operation as a probe.
- In `HalfOpen`, a **successful** probe transitions the breaker to `Closed` and resets the consecutive-failure counter to zero.
- In `HalfOpen`, a **failed** probe transitions the breaker back to `Open`, records the new opening time (restarting the reset timeout), and re-throws the original exception.
- Only **one** probe is active in `HalfOpen`; additional concurrent calls are not modelled — the reference implementation is single-threaded.
- Rejected operations (invalid threshold, invalid timeout, call while open) **throw domain exceptions** with byte-identical messages across languages.

## Named Constants

- `DefaultFailureThreshold = 5`
- `DefaultResetTimeout = 30 seconds` (`TimeSpan.FromSeconds(30)` / `30_000 ms` / `timedelta(seconds=30)`)

The kata brief quotes `threshold=3, timeout=30s` in its worked example; the 5-failure default is the more common production value. Builders override both per scenario.

## Test Scenarios

### Construction

1. **A new breaker is in the Closed state**
2. **Breaker rejects non-positive failure threshold with BreakerThresholdInvalidException**
3. **Breaker rejects non-positive reset timeout with BreakerTimeoutInvalidException**

### Closed State

4. **Execute in Closed returns the operation's result on success**
5. **Execute in Closed re-throws the operation's exception on failure**
6. **A single failure in Closed does not trip the breaker**
7. **Reaching the failure threshold in Closed transitions to Open**
8. **A success in Closed resets the consecutive-failure counter**
9. **Consecutive failures below the threshold stay Closed even after many operations**

### Open State

10. **Execute in Open throws CircuitOpenException without calling the operation**
11. **The state remains Open after a fail-fast rejection**
12. **The CircuitOpenException message is "Circuit is open"**

### Open → HalfOpen Transition

13. **Execute before the reset timeout elapses still fails fast**
14. **Execute after the reset timeout elapses probes the operation in HalfOpen**
15. **A successful probe transitions HalfOpen to Closed**
16. **A successful probe resets the consecutive-failure counter**

### HalfOpen → Open Transition

17. **A failed probe transitions HalfOpen back to Open and re-throws**
18. **A failed probe restarts the reset timeout**

### End-to-End Cycle

19. **Closed → Open → HalfOpen → Closed round-trip from the kata brief example**
20. **Closed → Open → HalfOpen → Open cycle when the probe fails**
