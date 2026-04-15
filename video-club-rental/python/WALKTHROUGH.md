# Video Club Rental — Python Walkthrough

This document complements the [C# walkthrough](../csharp/WALKTHROUGH.md). The design is shared — `VideoClub` aggregate, value types for `Money` and `Age`, `Clock` and `Notifier` as collaborators, three builders in `tests/`. This file names the Python-specific choices.

## `Decimal`, Not `float`, for Money

Python's `float` has the same IEEE-754 problem as JavaScript's `number`: `0.1 + 0.2 != 0.3`. For money that's an unforced error. This implementation uses `decimal.Decimal` and stringly constructs (`Money("2.50")`) to avoid binary-float contamination:

```python
@dataclass(frozen=True)
class Money:
    amount: Decimal
    def __init__(self, amount: Decimal | int | float | str = 0) -> None:
        object.__setattr__(self, "amount", Decimal(str(amount)))
```

`Money("2.50") + Money("2.25") + Money("1.75")` equals `Money("6.50")` exactly, and `==` on the frozen dataclass just works. Tests assert with `assert cost == Money("2.50")` — no rounding dance required.

The `frozen=True` dataclass gives structural equality and immutability for free; the custom `__init__` exists only to normalize input types into `Decimal`.

## `Protocol`, Not ABC, for Collaborators

`Clock` and `Notifier` are defined as `typing.Protocol`:

```python
class Clock(Protocol):
    def today(self) -> date: ...
```

`Protocol` is Python's structural-typing escape hatch — any object with a `today() -> date` method *is* a `Clock`, without inheriting. `FixedClock` in `tests/` never imports `Clock`; it just has the right shape. That's the same calculus as TypeScript's structural interfaces.

An `ABC` would have forced `FixedClock(Clock)` inheritance, which buys nothing here and loses the option of a throwaway inline clock (`types.SimpleNamespace(today=lambda: date(2026, 1, 1))`).

## `@dataclass(frozen=True)` for `Age` but Not `User`

`Age` is a value — two ages with the same years are equal, and an `Age` never mutates. Frozen dataclass is a one-liner that gives hashability, equality, and immutability:

```python
@dataclass(frozen=True)
class Age:
    years: int
    @property
    def is_adult(self) -> bool:
        return self.years >= AGE_ADULT_MINIMUM
```

`User`, `Title`, and `Rental` have identity and state. A regular class with explicit private attributes and read-only `@property` accessors is the right fit — equality is reference-based (the `User` you rented to *is* the `User` you refund to), not value-based.

## Package Re-exports via `__init__.py`

Every public type is re-exported from `video_club_rental/__init__.py`:

```python
from .age import Age, AGE_ADULT_MINIMUM
from .video_club import VideoClub, PRIORITY_ACCESS_THRESHOLD, ...
```

Tests import `from video_club_rental import Age, Money, VideoClub` without caring about internal module layout. If `video_club.py` grows and splits later, the public surface stays stable.

This is the idiomatic "small package, clean surface" shape — different from C# namespaces (everything in `VideoClubRental` is exportable by default) and TypeScript (explicit per-file `import`).

## Module-Level Constants

Like TypeScript, Python has no static-class idiom. `PRIORITY_ACCESS_THRESHOLD`, `DONATION_LOYALTY_AWARD`, `RENTAL_PERIOD_DAYS`, `BASE_PRICE` are all module-level `SCREAMING_SNAKE_CASE` exports. That reads naturally to Python developers: "constants belong at module top."

`pricing_policy` is a plain module, not a class. `pricing_policy.calculate(1, existing)` at the call site has the same shape as the C# `PricingPolicy.Calculate(1, existing)`.

## No `internal` — Convention Plus Comments

Python has no `internal` access modifier. `User.seed_priority_points` carries a `# Test-only` comment; `User.award_priority_points` is public because the aggregate needs it. In practice, the contract is "if you're not `VideoClub` or a test builder, don't call these." Python relies on convention at this point in its history, and adding runtime guards would cost more than it saves.

Leading-underscore methods (`_active_rentals_for`, `_require_title`) mark the truly-private aggregate helpers — same as every Python codebase.

## Test Builders as Plain Classes

Each builder (`UserBuilder`, `TitleBuilder`, `VideoClubBuilder`) is a plain class with fluent methods returning `self`. `VideoClubBuilder.build()` returns a three-tuple `(VideoClub, RecordingNotifier, FixedClock)` — unpacked at the call site as `club, notifier, clock = VideoClubBuilder()...build()`. Tuple-unpack is idiomatic Python; an object-return would feel foreign.

## Scenario Map

The twenty-four scenarios live across six test modules in `tests/`, one `def test_...` per scenario, names matching the SCENARIOS titles:

- `test_registration.py`
- `test_rental_pricing.py`
- `test_returns.py`
- `test_priority_access.py`
- `test_donations.py`
- `test_wishlist.py`

## How to Run

```bash
cd video-club-rental/python
python3.11 -m venv .venv
.venv/bin/pip install pytest
.venv/bin/pytest
```
