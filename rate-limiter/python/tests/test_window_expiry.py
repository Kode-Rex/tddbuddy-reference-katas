from __future__ import annotations

from datetime import timedelta

from rate_limiter import Allowed, Rejected

from .limiter_builder import LimiterBuilder

TEN_SECONDS = timedelta(seconds=10)


def test_a_request_exactly_at_the_window_boundary_opens_a_fresh_window_and_is_allowed():
    limiter, clock = LimiterBuilder().with_max_requests(3).with_window(TEN_SECONDS).build()
    limiter.request("alice")
    limiter.request("alice")
    limiter.request("alice")

    clock.advance(TEN_SECONDS)
    assert isinstance(limiter.request("alice"), Allowed)


def test_a_request_after_the_window_has_fully_elapsed_opens_a_fresh_window_and_is_allowed():
    limiter, clock = LimiterBuilder().with_max_requests(3).with_window(TEN_SECONDS).build()
    limiter.request("alice")
    limiter.request("alice")
    limiter.request("alice")

    clock.advance(timedelta(seconds=15))
    assert isinstance(limiter.request("alice"), Allowed)


def test_after_a_window_resets_the_full_quota_is_available_again():
    limiter, clock = LimiterBuilder().with_max_requests(3).with_window(TEN_SECONDS).build()
    limiter.request("alice")
    limiter.request("alice")
    limiter.request("alice")

    clock.advance(TEN_SECONDS)
    assert isinstance(limiter.request("alice"), Allowed)
    assert isinstance(limiter.request("alice"), Allowed)
    assert isinstance(limiter.request("alice"), Allowed)
    assert isinstance(limiter.request("alice"), Rejected)
