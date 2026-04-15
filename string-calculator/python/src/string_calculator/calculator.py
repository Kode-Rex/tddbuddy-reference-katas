import re

from string_calculator.delimiter_parser import parse


def add(numbers: str) -> int:
    if numbers == "":
        return 0
    parsed = parse(numbers)
    values = [int(token) for token in re.split(parsed.pattern, parsed.body)]
    negatives = [n for n in values if n < 0]
    if negatives:
        raise ValueError("negatives not allowed: " + ", ".join(str(n) for n in negatives))
    return sum(values)
