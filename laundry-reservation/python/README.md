# Laundry Reservation — Python Reference Implementation

- Python 3.11+
- pytest test runner
- dataclasses for value types
- Protocol for collaborator interfaces

## Build & Run

```bash
cd laundry-reservation/python
python -m venv .venv
source .venv/bin/activate
pip install -e ".[dev]"
pytest
```

See [`WALKTHROUGH.md`](WALKTHROUGH.md) and [`../SCENARIOS.md`](../SCENARIOS.md).
