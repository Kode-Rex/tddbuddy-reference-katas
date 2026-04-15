_ONES = (
    "zero", "one", "two", "three", "four",
    "five", "six", "seven", "eight", "nine",
    "ten", "eleven", "twelve", "thirteen", "fourteen",
    "fifteen", "sixteen", "seventeen", "eighteen", "nineteen",
)

_TENS = (
    "", "", "twenty", "thirty", "forty",
    "fifty", "sixty", "seventy", "eighty", "ninety",
)


def to_words(n: int) -> str:
    if n == 0:
        return "zero"

    parts: list[str] = []

    thousands, n = divmod(n, 1000)
    if thousands:
        parts.append(f"{_ONES[thousands]} thousand")

    hundreds, n = divmod(n, 100)
    if hundreds:
        parts.append(f"{_ONES[hundreds]} hundred")

    if n:
        parts.append(_below_hundred(n))

    return " ".join(parts)


def _below_hundred(n: int) -> str:
    if n < 20:
        return _ONES[n]
    tens, ones = divmod(n, 10)
    return _TENS[tens] if ones == 0 else f"{_TENS[tens]}-{_ONES[ones]}"
