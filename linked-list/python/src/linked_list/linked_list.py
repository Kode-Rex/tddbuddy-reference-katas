from __future__ import annotations

from typing import Generic, Optional, TypeVar

T = TypeVar("T")


class _Node(Generic[T]):
    __slots__ = ("value", "next")

    def __init__(self, value: T, next: Optional["_Node[T]"] = None) -> None:
        self.value = value
        self.next = next


class LinkedList(Generic[T]):
    def __init__(self) -> None:
        self._head: Optional[_Node[T]] = None
        self._count: int = 0

    def size(self) -> int:
        return self._count

    def append(self, value: T) -> None:
        node = _Node(value)
        if self._head is None:
            self._head = node
        else:
            current = self._head
            while current.next is not None:
                current = current.next
            current.next = node
        self._count += 1

    def prepend(self, value: T) -> None:
        self._head = _Node(value, self._head)
        self._count += 1

    def get(self, index: int) -> T:
        return self._node_at(index).value

    def remove(self, index: int) -> T:
        if index < 0 or index >= self._count:
            raise _out_of_range(index)
        if index == 0:
            assert self._head is not None
            removed = self._head
            self._head = removed.next
        else:
            previous = self._node_at(index - 1)
            assert previous.next is not None
            removed = previous.next
            previous.next = removed.next
        self._count -= 1
        return removed.value

    def insert_at(self, index: int, value: T) -> None:
        if index < 0 or index > self._count:
            raise _out_of_range(index)
        if index == 0:
            self.prepend(value)
            return
        if index == self._count:
            self.append(value)
            return
        previous = self._node_at(index - 1)
        previous.next = _Node(value, previous.next)
        self._count += 1

    def contains(self, value: T) -> bool:
        return self.index_of(value) >= 0

    def index_of(self, value: T) -> int:
        i = 0
        current = self._head
        while current is not None:
            if current.value == value:
                return i
            current = current.next
            i += 1
        return -1

    def to_array(self) -> list[T]:
        result: list[T] = []
        current = self._head
        while current is not None:
            result.append(current.value)
            current = current.next
        return result

    def _node_at(self, index: int) -> _Node[T]:
        if index < 0 or index >= self._count:
            raise _out_of_range(index)
        assert self._head is not None
        current = self._head
        for _ in range(index):
            assert current.next is not None
            current = current.next
        return current


def _out_of_range(index: int) -> IndexError:
    return IndexError(f"index out of range: {index}")
