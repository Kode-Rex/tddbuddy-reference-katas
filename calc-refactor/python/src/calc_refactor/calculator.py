from typing import Optional


# Identical byte-for-byte across C#, TypeScript, and Python.
# See ../../SCENARIOS.md for the characterization contract.
class KEYS:
    EQUALS = "="
    CLEAR = "C"


class DISPLAY:
    ZERO = "0"
    ERROR = "Error"


class ERRORS:
    @staticmethod
    def unknown_key(key: str) -> str:
        return f"unknown key: {key}"


_OPERATORS = frozenset({"+", "-", "*", "/"})


def _is_digit(c: str) -> bool:
    return len(c) == 1 and "0" <= c <= "9"


def _trunc_div(a: int, b: int) -> int:
    # Truncate toward zero — matches C# `/` and TS Math.trunc(a/b).
    # Python's `//` floors toward -inf, so we correct the sign here.
    return int(a / b) if b != 0 else 0


class Calculator:
    def __init__(self) -> None:
        self._display: str = DISPLAY.ZERO
        self._left: int = 0
        self._pending_operator: Optional[str] = None
        self._remembered_right: int = 0
        self._has_remembered_right: bool = False
        self._entering_operand: bool = False
        self._just_evaluated: bool = False
        self._in_error: bool = False

    @property
    def display(self) -> str:
        return self._display

    def press(self, key: str) -> None:
        if self._in_error:
            if key == KEYS.CLEAR:
                self._reset()
            return

        if _is_digit(key):
            self._press_digit(key)
            return
        if key in _OPERATORS:
            self._press_operator(key)
            return
        if key == KEYS.EQUALS:
            self._press_equals()
            return
        if key == KEYS.CLEAR:
            self._reset()
            return

        raise ValueError(ERRORS.unknown_key(key))

    def _press_digit(self, key: str) -> None:
        if not self._entering_operand or self._just_evaluated:
            if self._just_evaluated:
                # A digit after `=` starts a fresh calculation — drop the
                # accumulator and the pending operator so the new operand
                # is not folded into the remembered state.
                self._left = 0
                self._pending_operator = None
                self._has_remembered_right = False
            self._display = key
            self._entering_operand = True
            self._just_evaluated = False
            return

        self._display = key if self._display == DISPLAY.ZERO else self._display + key

    def _press_operator(self, key: str) -> None:
        if self._entering_operand or self._just_evaluated:
            if self._pending_operator is not None and self._entering_operand:
                self._left = self._apply(self._left, self._pending_operator, int(self._display))
                self._display = str(self._left)
                if self._in_error:
                    return
            else:
                self._left = int(self._display)
            self._pending_operator = key
            self._entering_operand = False
            self._just_evaluated = False
            self._has_remembered_right = False
            return

        # Two operators in a row — the legacy `1++2` path.
        self._enter_error()

    def _press_equals(self) -> None:
        if self._pending_operator is None:
            return

        right = (
            self._remembered_right
            if self._has_remembered_right and not self._entering_operand
            else int(self._display)
        )

        result = self._apply(self._left, self._pending_operator, right)
        if self._in_error:
            return

        self._remembered_right = right
        self._has_remembered_right = True
        self._left = result
        self._display = str(result)
        self._entering_operand = False
        self._just_evaluated = True

    def _apply(self, left: int, op: str, right: int) -> int:
        if op == "+":
            return left + right
        if op == "-":
            return left - right
        if op == "*":
            return left * right
        if op == "/":
            if right == 0:
                self._enter_error()
                return 0
            return _trunc_div(left, right)
        raise AssertionError(f"unreachable operator: {op}")

    def _enter_error(self) -> None:
        self._in_error = True
        self._display = DISPLAY.ERROR

    def _reset(self) -> None:
        self._display = DISPLAY.ZERO
        self._left = 0
        self._pending_operator = None
        self._remembered_right = 0
        self._has_remembered_right = False
        self._entering_operand = False
        self._just_evaluated = False
        self._in_error = False
