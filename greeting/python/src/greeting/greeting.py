from __future__ import annotations


def greet(name: str | list[str | None] | None) -> str:
    if isinstance(name, list):
        names = [n if n is not None else "my friend" for n in name]
    else:
        names = [name if name is not None else "my friend"]

    normals: list[str] = []
    shouts: list[str] = []
    for n in names:
        if _is_shout(n):
            shouts.append(n)
        else:
            normals.append(n)

    if not shouts:
        return _normal_greeting(normals)
    if not normals:
        return _shout_greeting(shouts)
    return f"{_normal_greeting(normals)}. AND {_shout_greeting(shouts)}"


def _normal_greeting(names: list[str]) -> str:
    if len(names) == 1:
        return f"Hello, {names[0]}."
    if len(names) == 2:
        return f"Hello, {names[0]} and {names[1]}"
    head = ", ".join(names[:-1])
    return f"Hello, {head}, and {names[-1]}"


def _shout_greeting(names: list[str]) -> str:
    return f"HELLO {' '.join(names)}!"


def _is_shout(name: str) -> bool:
    return any(c.isalpha() for c in name) and name == name.upper()
