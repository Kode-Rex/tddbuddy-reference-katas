def print_diamond(letter: str) -> str:
    if len(letter) != 1:
        raise ValueError("letter must be a single A-Z character")
    normalized = letter.upper()
    if not ("A" <= normalized <= "Z"):
        raise ValueError("letter must be a single A-Z character")

    n = ord(normalized) - ord("A")
    rows: list[str] = []
    for r in range(2 * n + 1):
        offset = r if r <= n else 2 * n - r
        rows.append(_build_row(offset, n))
    return "\n".join(rows)


def _build_row(offset: int, n: int) -> str:
    row_letter = chr(ord("A") + offset)
    leading = " " * (n - offset)
    if offset == 0:
        return leading + row_letter
    inner = " " * (2 * offset - 1)
    return leading + row_letter + inner + row_letter
