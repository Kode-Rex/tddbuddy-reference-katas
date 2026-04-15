# Kata Potter — Python Reference Implementation

- Python 3.11+
- pytest

## Build & Run

```bash
cd kata-potter/python
python3.11 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```

See [`WALKTHROUGH.md`](WALKTHROUGH.md) for Python-specific notes, the [C# walkthrough](../csharp/WALKTHROUGH.md) for the full design rationale, and [`../SCENARIOS.md`](../SCENARIOS.md) for the shared specification.
