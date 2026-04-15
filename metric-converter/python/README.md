# Metric Converter — Python Reference Implementation

- Python 3.11+
- pytest for the test runner

## Build & Run

```bash
cd metric-converter/python
python3.11 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```

See [`WALKTHROUGH.md`](WALKTHROUGH.md) for the design note and [`../SCENARIOS.md`](../SCENARIOS.md) for the shared specification.
