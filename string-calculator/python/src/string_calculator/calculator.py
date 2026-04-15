import re


def add(numbers: str) -> int:
    if numbers == "":
        return 0

    delimiter_pattern = r"[,\n]"
    body = numbers

    if numbers.startswith("//"):
        header_end = numbers.index("\n")
        header = numbers[2:header_end]
        delimiter_pattern = re.escape(header)
        body = numbers[header_end + 1:]

    return sum(int(token) for token in re.split(delimiter_pattern, body))
