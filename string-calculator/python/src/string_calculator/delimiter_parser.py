import re
from typing import NamedTuple


DEFAULT_DELIMITER_PATTERN = r"[,\n]"


class ParsedInput(NamedTuple):
    pattern: str
    body: str


def parse(raw: str) -> ParsedInput:
    if not raw.startswith("//"):
        return ParsedInput(DEFAULT_DELIMITER_PATTERN, raw)

    header_end = raw.index("\n")
    header = raw[2:header_end]
    body = raw[header_end + 1:]
    return ParsedInput(re.escape(header), body)
