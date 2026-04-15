# Video Club Rental — Python Reference Implementation

- Python 3.11+
- pytest for the test runner
- `src/video_club_rental/` package layout
- `dataclass(frozen=True)` value types; `Protocol` collaborators; `Decimal` money

## Build & Run

```bash
cd video-club-rental/python
python3.11 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```

See [`WALKTHROUGH.md`](WALKTHROUGH.md) and [`../SCENARIOS.md`](../SCENARIOS.md).
