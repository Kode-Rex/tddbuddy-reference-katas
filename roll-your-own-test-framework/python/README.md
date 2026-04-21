# Roll Your Own Test Framework — Python Reference Implementation

- Python 3.11+
- pytest for the test runner
- `src/roll_your_own_test_framework/` package layout

## Build & Run

```bash
cd roll-your-own-test-framework/python
python3 -m venv .venv
.venv/bin/pip install -e ".[dev]"
.venv/bin/pytest
```

See [`WALKTHROUGH.md`](WALKTHROUGH.md) for design rationale and [`../SCENARIOS.md`](../SCENARIOS.md) for the shared specification.
