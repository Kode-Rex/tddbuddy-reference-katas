# Bingo — Python Reference Implementation

- Python 3.11+
- pytest for the test runner

## Build & Run

```bash
cd bingo/python
python3.11 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```

See [`WALKTHROUGH.md`](WALKTHROUGH.md) for design rationale, [`../SCENARIOS.md`](../SCENARIOS.md) for the shared specification, and [`../README.md`](../README.md) for kata scope.
