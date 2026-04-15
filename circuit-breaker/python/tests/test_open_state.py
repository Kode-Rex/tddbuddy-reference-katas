from __future__ import annotations

from datetime import timedelta

import pytest

from circuit_breaker import BreakerState, CircuitOpenError

from .breaker_builder import BreakerBuilder

THIRTY_SECONDS = timedelta(seconds=30)


def _fails():
    def op():
        raise RuntimeError("boom")
    return op


def _swallow(fn):
    try:
        fn()
    except Exception:
        pass


def _tripped():
    breaker, clock = (
        BreakerBuilder().with_threshold(3).with_timeout(THIRTY_SECONDS).build()
    )
    for _ in range(3):
        _swallow(lambda: breaker.execute(_fails()))
    return breaker, clock


def test_execute_in_Open_throws_CircuitOpenError_without_calling_the_operation():
    breaker, _ = _tripped()
    called = {"v": False}

    def probe():
        called["v"] = True
        return "unused"

    with pytest.raises(CircuitOpenError):
        breaker.execute(probe)
    assert called["v"] is False


def test_the_state_remains_Open_after_a_fail_fast_rejection():
    breaker, _ = _tripped()
    _swallow(lambda: breaker.execute(lambda: "unused"))
    assert breaker.state is BreakerState.OPEN


def test_the_CircuitOpenError_message_is_Circuit_is_open():
    breaker, _ = _tripped()
    with pytest.raises(CircuitOpenError) as excinfo:
        breaker.execute(lambda: "unused")
    assert str(excinfo.value) == "Circuit is open"
