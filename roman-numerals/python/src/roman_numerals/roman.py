_LOOKUP = {
    1: "I",
    2: "II",
    3: "III",
}


def to_roman(n: int) -> str:
    return _LOOKUP[n]
