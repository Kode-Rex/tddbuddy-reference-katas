# Supermarket Pricing — Python Reference Implementation

- Python 3.11+
- pytest
- `src/supermarket_pricing/` package layout with `pyproject.toml`

## Build & Run

```bash
cd supermarket-pricing/python
python3 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```

See [`WALKTHROUGH.md`](WALKTHROUGH.md) for the design rationale and [`../SCENARIOS.md`](../SCENARIOS.md) for the shared specification.
