from __future__ import annotations

from datetime import datetime, timedelta, timezone

from rate_limiter import Allowed, Rejected

from .limiter_builder import LimiterBuilder

TEN_SECONDS = timedelta(seconds=10)


def test_fixed_window_cycle_for_alice_and_bob_produces_the_documented_sequence():
    start = datetime(2026, 1, 1, tzinfo=timezone.utc)
    limiter, clock = (
        LimiterBuilder()
        .with_max_requests(3)
        .with_window(TEN_SECONDS)
        .starting_at(start)
        .build()
    )

    # t=0,1,2 — alice allowed
    assert isinstance(limiter.request("alice"), Allowed)
    clock.advance(timedelta(seconds=1))
    assert isinstance(limiter.request("alice"), Allowed)
    clock.advance(timedelta(seconds=1))
    assert isinstance(limiter.request("alice"), Allowed)

    # t=3 — alice rejected with retry_after=7s
    clock.advance(timedelta(seconds=1))
    assert limiter.request("alice") == Rejected(timedelta(seconds=7))

    # t=3 — bob allowed (independent)
    assert isinstance(limiter.request("bob"), Allowed)

    # t=10 — alice's window has elapsed; fresh quota
    clock.advance(timedelta(seconds=7))
    assert isinstance(limiter.request("alice"), Allowed)
    assert isinstance(limiter.request("alice"), Allowed)
    assert isinstance(limiter.request("alice"), Allowed)
    assert isinstance(limiter.request("alice"), Rejected)
