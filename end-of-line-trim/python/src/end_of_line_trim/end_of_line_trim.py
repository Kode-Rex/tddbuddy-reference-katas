from __future__ import annotations


def trim(input: str) -> str:
    parts: list[str] = []
    line_start = 0
    i = 0
    n = len(input)
    while i < n:
        ch = input[i]
        if ch == "\n":
            parts.append(_right_trim(input, line_start, i))
            parts.append("\n")
            line_start = i + 1
            i += 1
        elif ch == "\r" and i + 1 < n and input[i + 1] == "\n":
            parts.append(_right_trim(input, line_start, i))
            parts.append("\r\n")
            line_start = i + 2
            i += 2
        else:
            i += 1
    parts.append(_right_trim(input, line_start, n))
    return "".join(parts)


def _right_trim(input: str, start: int, end: int) -> str:
    trimmed_end = end
    while trimmed_end > start and input[trimmed_end - 1] in (" ", "\t"):
        trimmed_end -= 1
    return input[start:trimmed_end]
