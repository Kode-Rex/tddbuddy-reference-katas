# Multi-Threaded Santa — Python Reference Implementation

- Python 3.11+
- pytest
- `threading` for concurrent workers
- `queue.Queue` for bounded queues (built-in bounded support)
- `threading.Lock` for sleigh exclusion

## Build & Run

```bash
cd multi-threaded-santa/python
python3 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```

See [`WALKTHROUGH.md`](WALKTHROUGH.md) and [`../SCENARIOS.md`](../SCENARIOS.md).
