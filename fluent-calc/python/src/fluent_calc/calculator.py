from __future__ import annotations

from dataclasses import dataclass
from typing import Literal

Op = Literal["plus", "minus"]


@dataclass(frozen=True)
class _Operation:
    kind: Op
    operand: int


class Calculator:
    def __init__(self) -> None:
        self._value: int = 0
        self._seeded: bool = False
        self._undo: list[_Operation] = []
        self._redo: list[_Operation] = []

    def seed(self, n: int) -> "Calculator":
        if self._seeded:
            return self
        self._value = n
        self._seeded = True
        return self

    def plus(self, n: int) -> "Calculator":
        return self._apply(_Operation("plus", n))

    def minus(self, n: int) -> "Calculator":
        return self._apply(_Operation("minus", n))

    def undo(self) -> "Calculator":
        if not self._undo:
            return self
        operation = self._undo.pop()
        self._value = _reverse(self._value, operation)
        self._redo.append(operation)
        return self

    def redo(self) -> "Calculator":
        if not self._redo:
            return self
        operation = self._redo.pop()
        self._value = _forward(self._value, operation)
        self._undo.append(operation)
        return self

    def save(self) -> "Calculator":
        self._undo.clear()
        self._redo.clear()
        return self

    def result(self) -> int:
        return self._value

    def _apply(self, operation: _Operation) -> "Calculator":
        if not self._seeded:
            return self
        self._value = _forward(self._value, operation)
        self._undo.append(operation)
        self._redo.clear()
        return self


def _forward(value: int, operation: _Operation) -> int:
    return value + operation.operand if operation.kind == "plus" else value - operation.operand


def _reverse(value: int, operation: _Operation) -> int:
    return value - operation.operand if operation.kind == "plus" else value + operation.operand
