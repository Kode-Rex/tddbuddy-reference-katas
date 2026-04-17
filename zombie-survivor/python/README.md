# Zombie Survivor — Python Reference Implementation

- Python 3.11+
- pytest for the test runner
- dataclasses for value types

## Build & Run

```bash
cd zombie-survivor/python
python3 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```

See [`WALKTHROUGH.md`](WALKTHROUGH.md) for Python-specific notes and [`../SCENARIOS.md`](../SCENARIOS.md) for the shared specification.
