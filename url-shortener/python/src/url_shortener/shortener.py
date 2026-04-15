from dataclasses import dataclass

SHORT_URL_BASE = "https://short.url/"

_BASE36_ALPHABET = "0123456789abcdefghijklmnopqrstuvwxyz"


@dataclass(frozen=True)
class UrlStatistics:
    short_url: str
    long_url: str
    visits: int


class Shortener:
    def __init__(self) -> None:
        self._long_to_code: dict[str, str] = {}
        self._code_to_long: dict[str, str] = {}
        self._visits: dict[str, int] = {}
        self._next_counter = 0

    def shorten(self, long_url: str) -> str:
        existing = self._long_to_code.get(long_url)
        if existing is not None:
            return SHORT_URL_BASE + existing

        code = _to_base36(self._next_counter)
        self._next_counter += 1
        self._long_to_code[long_url] = code
        self._code_to_long[code] = long_url
        self._visits[code] = 0
        return SHORT_URL_BASE + code

    def translate(self, url: str) -> str:
        short_code = self._resolve_by_short_url(url)
        if short_code is not None:
            self._visits[short_code] += 1
            return SHORT_URL_BASE + short_code

        code_from_long = self._long_to_code.get(url)
        if code_from_long is not None:
            return SHORT_URL_BASE + code_from_long

        raise ValueError(f"Unknown URL: {url}")

    def statistics(self, url: str) -> UrlStatistics:
        short_code = self._resolve_by_short_url(url)
        if short_code is not None:
            return self._build_statistics(short_code)

        code_from_long = self._long_to_code.get(url)
        if code_from_long is not None:
            return self._build_statistics(code_from_long)

        raise ValueError(f"Unknown URL: {url}")

    def _resolve_by_short_url(self, url: str) -> str | None:
        if not url.startswith(SHORT_URL_BASE):
            return None
        candidate = url[len(SHORT_URL_BASE):]
        return candidate if candidate in self._code_to_long else None

    def _build_statistics(self, code: str) -> UrlStatistics:
        return UrlStatistics(
            short_url=SHORT_URL_BASE + code,
            long_url=self._code_to_long[code],
            visits=self._visits[code],
        )


def _to_base36(value: int) -> str:
    if value == 0:
        return "0"
    digits: list[str] = []
    while value > 0:
        digits.append(_BASE36_ALPHABET[value % 36])
        value //= 36
    return "".join(reversed(digits))
