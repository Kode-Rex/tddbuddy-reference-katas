from dataclasses import dataclass, field
from typing import List


# Identical byte-for-byte across C#, TypeScript, and Python.
# The failure message strings are the spec (see ../SCENARIOS.md).
class RuleNames:
    MINIMUM_LENGTH = "minimum length"
    REQUIRES_DIGIT = "requires digit"
    REQUIRES_SYMBOL = "requires symbol"
    REQUIRES_UPPERCASE = "requires uppercase"
    REQUIRES_LOWERCASE = "requires lowercase"


DEFAULT_MIN_LENGTH = 8


@dataclass(frozen=True)
class ValidationResult:
    ok: bool
    failures: List[str] = field(default_factory=list)


@dataclass(frozen=True)
class Policy:
    min_length: int
    requires_digit: bool = False
    requires_symbol: bool = False
    requires_upper: bool = False
    requires_lower: bool = False

    def validate(self, password: str) -> ValidationResult:
        failures: List[str] = []
        if len(password) < self.min_length:
            failures.append(RuleNames.MINIMUM_LENGTH)
        if self.requires_digit and not any(_is_digit(c) for c in password):
            failures.append(RuleNames.REQUIRES_DIGIT)
        if self.requires_symbol and not any(_is_symbol(c) for c in password):
            failures.append(RuleNames.REQUIRES_SYMBOL)
        if self.requires_upper and not any(_is_upper(c) for c in password):
            failures.append(RuleNames.REQUIRES_UPPERCASE)
        if self.requires_lower and not any(_is_lower(c) for c in password):
            failures.append(RuleNames.REQUIRES_LOWERCASE)
        return ValidationResult(ok=len(failures) == 0, failures=failures)


def _is_digit(c: str) -> bool:
    return "0" <= c <= "9"


def _is_upper(c: str) -> bool:
    return "A" <= c <= "Z"


def _is_lower(c: str) -> bool:
    return "a" <= c <= "z"


def _is_symbol(c: str) -> bool:
    return not (_is_digit(c) or _is_upper(c) or _is_lower(c))
