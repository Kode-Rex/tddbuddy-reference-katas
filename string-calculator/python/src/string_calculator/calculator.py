import re

from string_calculator.delimiter_parser import parse


def add(numbers: str) -> int:
    if numbers == "":
        return 0
    parsed = parse(numbers)
    return sum(int(token) for token in re.split(parsed.pattern, parsed.body))
