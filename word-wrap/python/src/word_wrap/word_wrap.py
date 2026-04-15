from __future__ import annotations


def wrap(text: str, width: int) -> str:
    words = text.split()
    if not words:
        return ""

    lines: list[str] = []
    line = ""

    for original in words:
        word = original
        while len(word) > width:
            if line:
                lines.append(line)
                line = ""
            lines.append(word[:width])
            word = word[width:]

        if not line:
            line = word
        elif len(line) + 1 + len(word) <= width:
            line = line + " " + word
        else:
            lines.append(line)
            line = word

    if line:
        lines.append(line)

    return "\n".join(lines)
