from __future__ import annotations

from datetime import timedelta

import pytest

from circuit_breaker import (
    BreakerState,
    BreakerThresholdInvalidError,
    BreakerTimeoutInvalidError,
)

from .breaker_builder import BreakerBuilder


def test_a_new_breaker_is_in_the_Closed_state():
    breaker, _ = BreakerBuilder().build()
    assert breaker.state is BreakerState.CLOSED


def test_breaker_rejects_non_positive_failure_threshold_with_BreakerThresholdInvalidError():
    with pytest.raises(BreakerThresholdInvalidError) as excinfo:
        BreakerBuilder().with_threshold(0).build()
    assert str(excinfo.value) == "Failure threshold must be positive"


def test_breaker_rejects_non_positive_reset_timeout_with_BreakerTimeoutInvalidError():
    with pytest.raises(BreakerTimeoutInvalidError) as excinfo:
        BreakerBuilder().with_timeout(timedelta(0)).build()
    assert str(excinfo.value) == "Reset timeout must be positive"
