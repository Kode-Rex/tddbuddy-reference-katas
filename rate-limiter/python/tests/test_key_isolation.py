from __future__ import annotations

from datetime import timedelta

from rate_limiter import Allowed, Rejected

from .limiter_builder import LimiterBuilder

TEN_SECONDS = timedelta(seconds=10)


def test_two_different_keys_have_independent_counts():
    limiter, _ = LimiterBuilder().with_max_requests(3).with_window(TEN_SECONDS).build()

    assert isinstance(limiter.request("alice"), Allowed)
    assert isinstance(limiter.request("alice"), Allowed)
    assert isinstance(limiter.request("alice"), Allowed)

    assert isinstance(limiter.request("bob"), Allowed)
    assert isinstance(limiter.request("bob"), Allowed)
    assert isinstance(limiter.request("bob"), Allowed)


def test_a_rejection_on_one_key_does_not_affect_another_keys_decisions():
    limiter, _ = LimiterBuilder().with_max_requests(3).with_window(TEN_SECONDS).build()
    limiter.request("alice")
    limiter.request("alice")
    limiter.request("alice")
    assert isinstance(limiter.request("alice"), Rejected)

    assert isinstance(limiter.request("bob"), Allowed)


def test_each_keys_window_starts_from_its_own_first_request():
    limiter, clock = LimiterBuilder().with_max_requests(3).with_window(TEN_SECONDS).build()
    limiter.request("alice")
    limiter.request("alice")
    limiter.request("alice")

    clock.advance(timedelta(seconds=5))
    limiter.request("bob")
    limiter.request("bob")
    limiter.request("bob")

    assert limiter.request("alice") == Rejected(timedelta(seconds=5))
    assert limiter.request("bob") == Rejected(timedelta(seconds=10))
