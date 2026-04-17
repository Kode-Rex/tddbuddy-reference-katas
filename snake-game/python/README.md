# Snake Game — Python Reference Implementation

- Python 3.11+
- pytest for the test runner
- Frozen dataclasses for value types

## Build & Run

```bash
cd snake-game/python
python3 -m venv .venv
.venv/bin/pip install -e ".[dev]"
.venv/bin/pytest
```

See [`WALKTHROUGH.md`](WALKTHROUGH.md) for the design rationale and [`../SCENARIOS.md`](../SCENARIOS.md) for the shared specification.
