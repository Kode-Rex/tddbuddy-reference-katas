from __future__ import annotations

from datetime import timedelta

from rate_limiter import Allowed, Rejected

from .limiter_builder import LimiterBuilder

TEN_SECONDS = timedelta(seconds=10)


def test_the_first_request_for_a_key_is_allowed():
    limiter, _ = LimiterBuilder().with_max_requests(3).with_window(TEN_SECONDS).build()
    assert isinstance(limiter.request("alice"), Allowed)


def test_requests_up_to_the_limit_within_the_window_are_all_allowed():
    limiter, _ = LimiterBuilder().with_max_requests(3).with_window(TEN_SECONDS).build()
    assert isinstance(limiter.request("alice"), Allowed)
    assert isinstance(limiter.request("alice"), Allowed)
    assert isinstance(limiter.request("alice"), Allowed)


def test_each_allowed_decision_carries_no_retry_after():
    limiter, _ = LimiterBuilder().with_max_requests(3).with_window(TEN_SECONDS).build()
    decision = limiter.request("alice")
    assert isinstance(decision, Allowed)
    assert not hasattr(decision, "retry_after")


def test_the_request_past_the_limit_within_the_window_is_rejected():
    limiter, _ = LimiterBuilder().with_max_requests(3).with_window(TEN_SECONDS).build()
    limiter.request("alice")
    limiter.request("alice")
    limiter.request("alice")
    assert isinstance(limiter.request("alice"), Rejected)


def test_a_rejection_reports_retry_after_as_the_remaining_window_duration():
    limiter, clock = LimiterBuilder().with_max_requests(3).with_window(TEN_SECONDS).build()
    limiter.request("alice")
    limiter.request("alice")
    limiter.request("alice")

    clock.advance(timedelta(seconds=3))
    decision = limiter.request("alice")

    assert decision == Rejected(timedelta(seconds=7))


def test_a_rejected_request_does_not_count_against_the_window():
    limiter, clock = LimiterBuilder().with_max_requests(3).with_window(TEN_SECONDS).build()
    limiter.request("alice")
    limiter.request("alice")
    limiter.request("alice")

    for _ in range(5):
        limiter.request("alice")

    clock.advance(TEN_SECONDS)
    assert isinstance(limiter.request("alice"), Allowed)
    assert isinstance(limiter.request("alice"), Allowed)
    assert isinstance(limiter.request("alice"), Allowed)
    assert isinstance(limiter.request("alice"), Rejected)


def test_repeated_rejections_report_a_decreasing_retry_after_as_the_clock_advances():
    limiter, clock = LimiterBuilder().with_max_requests(3).with_window(TEN_SECONDS).build()
    limiter.request("alice")
    limiter.request("alice")
    limiter.request("alice")

    clock.advance(timedelta(seconds=2))
    first = limiter.request("alice")

    clock.advance(timedelta(seconds=3))
    second = limiter.request("alice")

    assert first == Rejected(timedelta(seconds=8))
    assert second == Rejected(timedelta(seconds=5))
