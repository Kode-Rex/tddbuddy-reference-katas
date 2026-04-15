import re


def add(numbers: str) -> int:
    if numbers == "":
        return 0
    return sum(int(token) for token in re.split(r"[,\n]", numbers))
