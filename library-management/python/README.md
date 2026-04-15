# Library Management — Python Reference Implementation

- Python 3.11+
- pytest for the test runner
- `Decimal` for monetary values, `Protocol` for collaborator interfaces

## Build & Run

```bash
cd library-management/python
pip install -e '.[dev]'
pytest
```

See [`WALKTHROUGH.md`](WALKTHROUGH.md) for the design rationale and [`../SCENARIOS.md`](../SCENARIOS.md) for the shared specification.
