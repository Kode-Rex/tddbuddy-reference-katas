# Shopping Cart — Python Reference Implementation

- Python 3.11+
- pytest
- `src/shopping_cart/` package layout with `pyproject.toml`

## Build & Run

```bash
cd shopping-cart/python
python3 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```

See [`WALKTHROUGH.md`](WALKTHROUGH.md) for the design rationale and [`../SCENARIOS.md`](../SCENARIOS.md) for the shared specification.
