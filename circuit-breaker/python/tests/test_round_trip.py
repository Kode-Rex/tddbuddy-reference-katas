from __future__ import annotations

from datetime import timedelta

import pytest

from circuit_breaker import BreakerState, CircuitOpenError

from .breaker_builder import BreakerBuilder

THIRTY_SECONDS = timedelta(seconds=30)


def _succeeds(value):
    return lambda: value


def _fails():
    def op():
        raise RuntimeError("boom")
    return op


def _swallow(fn):
    try:
        fn()
    except Exception:
        pass


def test_Closed_Open_HalfOpen_Closed_round_trip_from_the_kata_brief_example():
    breaker, clock = (
        BreakerBuilder().with_threshold(3).with_timeout(THIRTY_SECONDS).build()
    )

    assert breaker.execute(_succeeds("ok")) == "ok"
    _swallow(lambda: breaker.execute(_fails()))
    _swallow(lambda: breaker.execute(_fails()))
    _swallow(lambda: breaker.execute(_fails()))
    assert breaker.state is BreakerState.OPEN

    with pytest.raises(CircuitOpenError):
        breaker.execute(_succeeds("ignored"))

    clock.advance(THIRTY_SECONDS)
    assert breaker.execute(_succeeds("ok")) == "ok"
    assert breaker.state is BreakerState.CLOSED


def test_Closed_Open_HalfOpen_Open_cycle_when_the_probe_fails():
    breaker, clock = (
        BreakerBuilder().with_threshold(3).with_timeout(THIRTY_SECONDS).build()
    )
    for _ in range(3):
        _swallow(lambda: breaker.execute(_fails()))
    clock.advance(THIRTY_SECONDS)

    with pytest.raises(RuntimeError, match="boom"):
        breaker.execute(_fails())
    assert breaker.state is BreakerState.OPEN
