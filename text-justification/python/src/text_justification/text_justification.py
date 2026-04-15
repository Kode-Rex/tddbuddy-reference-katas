from __future__ import annotations


def justify(text: str, width: int) -> list[str]:
    words = text.split()
    if not words:
        return []

    lines: list[str] = []
    line_words: list[str] = []
    line_content_length = 0

    for word in words:
        projected = line_content_length + len(line_words) + len(word)
        if line_words and projected > width:
            lines.append(_justify_line(line_words, line_content_length, width))
            line_words = []
            line_content_length = 0
        line_words.append(word)
        line_content_length += len(word)

    if line_words:
        lines.append(" ".join(line_words))

    return lines


def _justify_line(line_words: list[str], content_length: int, width: int) -> str:
    if len(line_words) == 1:
        only = line_words[0]
        return only if len(only) >= width else only + " " * (width - len(only))

    gaps = len(line_words) - 1
    padding = width - content_length
    base_spaces, extras = divmod(padding, gaps)

    parts: list[str] = []
    for i, word in enumerate(line_words):
        parts.append(word)
        if i < gaps:
            spaces = base_spaces + (1 if i < extras else 0)
            parts.append(" " * spaces)
    return "".join(parts)
