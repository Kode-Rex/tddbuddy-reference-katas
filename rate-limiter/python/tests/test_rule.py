from __future__ import annotations

from datetime import timedelta

import pytest

from rate_limiter import LimiterRuleInvalidError, Rule


def test_a_rule_with_positive_max_requests_and_positive_window_is_valid():
    rule = Rule(3, timedelta(seconds=10))
    assert rule.max_requests == 3
    assert rule.window == timedelta(seconds=10)


def test_a_rule_rejects_zero_max_requests_with_LimiterRuleInvalidError():
    with pytest.raises(LimiterRuleInvalidError) as excinfo:
        Rule(0, timedelta(seconds=10))
    assert str(excinfo.value) == "Max requests must be positive"


def test_a_rule_rejects_negative_max_requests_with_LimiterRuleInvalidError():
    with pytest.raises(LimiterRuleInvalidError) as excinfo:
        Rule(-1, timedelta(seconds=10))
    assert str(excinfo.value) == "Max requests must be positive"


def test_a_rule_rejects_zero_window_with_LimiterRuleInvalidError():
    with pytest.raises(LimiterRuleInvalidError) as excinfo:
        Rule(3, timedelta(0))
    assert str(excinfo.value) == "Window must be positive"


def test_a_rule_rejects_negative_window_with_LimiterRuleInvalidError():
    with pytest.raises(LimiterRuleInvalidError) as excinfo:
        Rule(3, timedelta(seconds=-1))
    assert str(excinfo.value) == "Window must be positive"
