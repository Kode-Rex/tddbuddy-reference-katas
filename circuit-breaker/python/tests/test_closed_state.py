from __future__ import annotations

import pytest

from circuit_breaker import BreakerState

from .breaker_builder import BreakerBuilder


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


def test_execute_in_Closed_returns_the_operations_result_on_success():
    breaker, _ = BreakerBuilder().with_threshold(3).build()
    assert breaker.execute(_succeeds("ok")) == "ok"
    assert breaker.state is BreakerState.CLOSED


def test_execute_in_Closed_rethrows_the_operations_exception_on_failure():
    breaker, _ = BreakerBuilder().with_threshold(3).build()
    with pytest.raises(RuntimeError, match="boom"):
        breaker.execute(_fails())


def test_a_single_failure_in_Closed_does_not_trip_the_breaker():
    breaker, _ = BreakerBuilder().with_threshold(3).build()
    _swallow(lambda: breaker.execute(_fails()))
    assert breaker.state is BreakerState.CLOSED


def test_reaching_the_failure_threshold_in_Closed_transitions_to_Open():
    breaker, _ = BreakerBuilder().with_threshold(3).build()
    for _ in range(3):
        _swallow(lambda: breaker.execute(_fails()))
    assert breaker.state is BreakerState.OPEN


def test_a_success_in_Closed_resets_the_consecutive_failure_counter():
    breaker, _ = BreakerBuilder().with_threshold(3).build()
    _swallow(lambda: breaker.execute(_fails()))
    _swallow(lambda: breaker.execute(_fails()))
    breaker.execute(_succeeds("ok"))
    _swallow(lambda: breaker.execute(_fails()))
    _swallow(lambda: breaker.execute(_fails()))
    assert breaker.state is BreakerState.CLOSED


def test_consecutive_failures_below_the_threshold_stay_Closed_even_after_many_operations():
    breaker, _ = BreakerBuilder().with_threshold(3).build()
    for _ in range(10):
        _swallow(lambda: breaker.execute(_fails()))
        breaker.execute(_succeeds("ok"))
    assert breaker.state is BreakerState.CLOSED
