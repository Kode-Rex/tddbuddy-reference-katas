from typing import Iterable


def _key(s: str) -> str:
    return "".join(sorted(c for c in s.lower() if c.isalpha()))


def are_anagrams(a: str, b: str) -> bool:
    if a == b:
        return False
    key_a = _key(a)
    key_b = _key(b)
    if not key_a or not key_b:
        return False
    return key_a == key_b


def find_anagrams(subject: str, candidates: Iterable[str]) -> list[str]:
    return [candidate for candidate in candidates if are_anagrams(subject, candidate)]


def group_anagrams(words: Iterable[str]) -> list[list[str]]:
    key_to_group: dict[str, list[str]] = {}
    order: list[str] = []
    for word in words:
        k = _key(word)
        if k not in key_to_group:
            key_to_group[k] = []
            order.append(k)
        key_to_group[k].append(word)
    return [key_to_group[k] for k in order]
