# Greeting — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Domain Rules

- `greet(name)` accepts `null`, a single name (string), or a list of names (array/list of strings).
- A **shout** is a name whose letters are all uppercase (it contains at least one letter and has no lowercase letters).
- `null` resolves to the stand-in name `"my friend"` and is greeted normally.
- A **single non-shout name** returns `"Hello, <name>."` (trailing period).
- A **single shout** returns `"HELLO <NAME>!"` (no comma, trailing bang).
- **Two non-shout names** return `"Hello, <a> and <b>"` (no trailing period).
- **Three or more non-shout names** return `"Hello, <a>, <b>, and <c>"` — Oxford comma before `and`, no trailing period.
- A **mix** of normals and shouts splits into two greetings joined with `". AND "`:
  - Normals first in their joined form (`"Hello, <normals…>"`),
  - then shouts in their joined form (`"HELLO <SHOUTS…>!"`),
  - e.g. `["Amy", "BRIAN", "Charlotte"] → "Hello, Amy and Charlotte. AND HELLO BRIAN!"`.

The output punctuation matches the kata prompt literally, including its per-scenario inconsistencies (scenarios 1–2 end with a period; scenarios 4–5 do not). The bonus rules (commas-in-entries and escape-via-quotes) are **not** implemented here — see the top-level [`README.md`](README.md).

## Test Scenarios

1. **Single name** — `"Bob"` returns `"Hello, Bob."`
2. **Null name** — `null` returns `"Hello, my friend."`
3. **Shouted name** — `"JERRY"` returns `"HELLO JERRY!"`
4. **Two names** — `["Jill", "Jane"]` returns `"Hello, Jill and Jane"`
5. **Three names with Oxford comma** — `["Amy", "Brian", "Charlotte"]` returns `"Hello, Amy, Brian, and Charlotte"`
6. **Mixed normals and shouts** — `["Amy", "BRIAN", "Charlotte"]` returns `"Hello, Amy and Charlotte. AND HELLO BRIAN!"`
