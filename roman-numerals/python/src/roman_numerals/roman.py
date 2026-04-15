_MAPPING: list[tuple[int, str]] = [
    (90, "XC"),
    (50, "L"),
    (40, "XL"),
    (10, "X"),
    (9, "IX"),
    (5, "V"),
    (4, "IV"),
    (1, "I"),
]


def to_roman(n: int) -> str:
    parts: list[str] = []
    for value, symbol in _MAPPING:
        while n >= value:
            parts.append(symbol)
            n -= value
    return "".join(parts)
