import pytest

from url_shortener import Shortener, UrlStatistics


def test_shorten_first_url_issues_code_zero():
    shortener = Shortener()
    assert shortener.shorten("https://example.com/alpha") == "https://short.url/0"


def test_second_distinct_url_issues_code_one():
    shortener = Shortener()
    shortener.shorten("https://example.com/alpha")
    assert shortener.shorten("https://example.com/beta") == "https://short.url/1"


def test_shortening_a_duplicate_returns_the_existing_short_url():
    shortener = Shortener()
    shortener.shorten("https://example.com/alpha")
    assert shortener.shorten("https://example.com/alpha") == "https://short.url/0"


def test_duplicate_does_not_advance_the_counter():
    shortener = Shortener()
    shortener.shorten("https://example.com/alpha")
    shortener.shorten("https://example.com/alpha")
    assert shortener.shorten("https://example.com/beta") == "https://short.url/1"


def test_eleventh_distinct_url_issues_code_a():
    shortener = Shortener()
    for i in range(10):
        shortener.shorten(f"https://example.com/url-{i}")
    assert shortener.shorten("https://example.com/url-10") == "https://short.url/a"


def test_translate_by_long_url_returns_the_short_url():
    shortener = Shortener()
    shortener.shorten("https://example.com/alpha")
    assert shortener.translate("https://example.com/alpha") == "https://short.url/0"


def test_translate_by_short_url_returns_the_same_short_url():
    shortener = Shortener()
    shortener.shorten("https://example.com/alpha")
    assert shortener.translate("https://short.url/0") == "https://short.url/0"


def test_translate_by_short_url_increments_visits():
    shortener = Shortener()
    shortener.shorten("https://example.com/alpha")
    shortener.translate("https://short.url/0")
    shortener.translate("https://short.url/0")
    shortener.translate("https://short.url/0")
    assert shortener.statistics("https://short.url/0").visits == 3


def test_translate_by_long_url_does_not_increment_visits():
    shortener = Shortener()
    shortener.shorten("https://example.com/alpha")
    shortener.translate("https://example.com/alpha")
    shortener.translate("https://example.com/alpha")
    shortener.translate("https://example.com/alpha")
    assert shortener.statistics("https://short.url/0").visits == 0


def test_shorten_does_not_count_as_a_visit():
    shortener = Shortener()
    shortener.shorten("https://example.com/alpha")
    assert shortener.statistics("https://short.url/0").visits == 0


def test_statistics_by_long_url():
    shortener = Shortener()
    shortener.shorten("https://example.com/alpha")
    shortener.translate("https://short.url/0")
    shortener.translate("https://short.url/0")
    assert shortener.statistics("https://example.com/alpha") == UrlStatistics(
        short_url="https://short.url/0",
        long_url="https://example.com/alpha",
        visits=2,
    )


def test_statistics_by_short_url():
    shortener = Shortener()
    shortener.shorten("https://example.com/alpha")
    shortener.translate("https://short.url/0")
    shortener.translate("https://short.url/0")
    assert shortener.statistics("https://short.url/0") == UrlStatistics(
        short_url="https://short.url/0",
        long_url="https://example.com/alpha",
        visits=2,
    )


def test_translate_on_unknown_url_raises():
    shortener = Shortener()
    with pytest.raises(ValueError, match=r"^Unknown URL: https://unknown\.example/x$"):
        shortener.translate("https://unknown.example/x")


def test_statistics_on_unknown_url_raises():
    shortener = Shortener()
    with pytest.raises(ValueError, match=r"^Unknown URL: https://unknown\.example/x$"):
        shortener.statistics("https://unknown.example/x")
