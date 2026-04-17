# Maze Walker — Python Reference Implementation

- Python 3.11+
- pytest for the test runner
- dataclasses for value types

## Build & Run

```bash
cd maze-walker/python
python -m venv .venv
source .venv/bin/activate
pip install -e ".[dev]"
pytest
```

See [`WALKTHROUGH.md`](WALKTHROUGH.md) for the design rationale and [`../SCENARIOS.md`](../SCENARIOS.md) for the shared specification.
