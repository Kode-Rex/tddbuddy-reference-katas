def is_valid(input: str) -> bool:
    if not input:
        return False

    octets = input.split(".")
    if len(octets) != 4:
        return False

    for i, octet in enumerate(octets):
        value = _parse_octet(octet)
        if value is None:
            return False
        if i == 3 and value in (0, 255):
            return False

    return True


def _parse_octet(octet: str) -> int | None:
    if len(octet) == 0:
        return None
    if len(octet) > 1 and octet[0] == "0":
        return None

    value = 0
    for c in octet:
        if not ("0" <= c <= "9"):
            return None
        value = value * 10 + (ord(c) - ord("0"))
        if value > 255:
            return None
    return value
