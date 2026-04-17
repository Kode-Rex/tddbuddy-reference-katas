# Bank OCR — Python Reference Implementation

- Python 3.11+
- pytest
- `@dataclass(frozen=True)` value types
- `src/bank_ocr/` package layout with `pyproject.toml`

## Build & Run

```bash
cd bank-ocr/python
python3 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```

See [`WALKTHROUGH.md`](WALKTHROUGH.md) for the Python-specific notes and [`../SCENARIOS.md`](../SCENARIOS.md) for the shared specification.
