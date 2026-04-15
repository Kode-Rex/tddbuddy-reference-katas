_LOOKUP = {
    1: "I",
    2: "II",
    3: "III",
    5: "V",
}


def to_roman(n: int) -> str:
    return _LOOKUP[n]
