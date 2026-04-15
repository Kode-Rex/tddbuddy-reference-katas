from dataclasses import dataclass
from typing import List, Protocol


# Byte-identical across C#, TypeScript, and Python. The truncation
# marker is part of the spec (see ../SCENARIOS.md).
TRUNCATION_MARKER = "..."


# Strategy. Every transformation is a self-contained rule — one input,
# one output, no state. Pipeline composes them; it does not know which
# rule any given strategy implements.
class Transformation(Protocol):
    def apply(self, input: str) -> str: ...


@dataclass(frozen=True)
class Pipeline:
    steps: List[Transformation]

    def run(self, input: str) -> str:
        current = input
        for step in self.steps:
            current = step.apply(current)
        return current


def _is_letter(c: str) -> bool:
    return ("a" <= c <= "z") or ("A" <= c <= "Z")


def _is_separator(c: str) -> bool:
    return c.isspace() or c == "-"


class Capitalise:
    def apply(self, input: str) -> str:
        if not input:
            return input
        chars = list(input)
        at_word_start = True
        for i, c in enumerate(chars):
            letter = _is_letter(c)
            if letter and at_word_start:
                chars[i] = c.upper()
            at_word_start = not letter
        return "".join(chars)


class Reverse:
    def apply(self, input: str) -> str:
        return input[::-1]


class RemoveWhitespace:
    def apply(self, input: str) -> str:
        return "".join(c for c in input if not c.isspace())


class SnakeCase:
    def apply(self, input: str) -> str:
        lowered = input.lower()
        out: List[str] = []
        in_separator = True  # suppress leading separators
        for c in lowered:
            if _is_separator(c):
                in_separator = True
            else:
                if in_separator and out:
                    out.append("_")
                out.append(c)
                in_separator = False
        return "".join(out)


class CamelCase:
    def apply(self, input: str) -> str:
        out: List[str] = []
        token_index = 0
        at_token_start = True
        for c in input:
            if _is_separator(c):
                if not at_token_start:
                    token_index += 1
                    at_token_start = True
            else:
                lower = c.lower()
                out.append(lower.upper() if at_token_start and token_index > 0 else lower)
                at_token_start = False
        return "".join(out)


@dataclass(frozen=True)
class Truncate:
    length: int

    def apply(self, input: str) -> str:
        if len(input) <= self.length:
            return input
        return input[: self.length] + TRUNCATION_MARKER


@dataclass(frozen=True)
class Repeat:
    times: int

    def apply(self, input: str) -> str:
        if self.times <= 0:
            return ""
        return " ".join([input] * self.times)


@dataclass(frozen=True)
class Replace:
    target: str
    replacement: str

    def apply(self, input: str) -> str:
        return input.replace(self.target, self.replacement)
