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


def _trip_to_open(breaker):
    for _ in range(3):
        _swallow(lambda: breaker.execute(_fails()))


def test_execute_before_the_reset_timeout_elapses_still_fails_fast():
    breaker, clock = (
        BreakerBuilder().with_threshold(3).with_timeout(THIRTY_SECONDS).build()
    )
    _trip_to_open(breaker)

    clock.advance(timedelta(seconds=29))

    with pytest.raises(CircuitOpenError):
        breaker.execute(_succeeds("ok"))


def test_execute_after_the_reset_timeout_elapses_probes_the_operation_in_HalfOpen():
    breaker, clock = (
        BreakerBuilder().with_threshold(3).with_timeout(THIRTY_SECONDS).build()
    )
    _trip_to_open(breaker)
    clock.advance(THIRTY_SECONDS)

    probed = {"v": False}

    def probe():
        probed["v"] = True
        return "ok"

    breaker.execute(probe)
    assert probed["v"] is True


def test_a_successful_probe_transitions_HalfOpen_to_Closed():
    breaker, clock = (
        BreakerBuilder().with_threshold(3).with_timeout(THIRTY_SECONDS).build()
    )
    _trip_to_open(breaker)
    clock.advance(THIRTY_SECONDS)

    breaker.execute(_succeeds("ok"))

    assert breaker.state is BreakerState.CLOSED


def test_a_successful_probe_resets_the_consecutive_failure_counter():
    breaker, clock = (
        BreakerBuilder().with_threshold(3).with_timeout(THIRTY_SECONDS).build()
    )
    _trip_to_open(breaker)
    clock.advance(THIRTY_SECONDS)
    breaker.execute(_succeeds("ok"))

    _swallow(lambda: breaker.execute(_fails()))
    _swallow(lambda: breaker.execute(_fails()))

    assert breaker.state is BreakerState.CLOSED


def test_a_failed_probe_transitions_HalfOpen_back_to_Open_and_rethrows():
    breaker, clock = (
        BreakerBuilder().with_threshold(3).with_timeout(THIRTY_SECONDS).build()
    )
    _trip_to_open(breaker)
    clock.advance(THIRTY_SECONDS)

    with pytest.raises(RuntimeError, match="boom"):
        breaker.execute(_fails())
    assert breaker.state is BreakerState.OPEN


def test_a_failed_probe_restarts_the_reset_timeout():
    breaker, clock = (
        BreakerBuilder().with_threshold(3).with_timeout(THIRTY_SECONDS).build()
    )
    _trip_to_open(breaker)
    clock.advance(THIRTY_SECONDS)
    _swallow(lambda: breaker.execute(_fails()))

    clock.advance(timedelta(seconds=29))
    with pytest.raises(CircuitOpenError):
        breaker.execute(_succeeds("ok"))
