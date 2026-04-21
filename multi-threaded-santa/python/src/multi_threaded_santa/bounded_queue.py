import queue
import threading
from typing import Generic, TypeVar

T = TypeVar("T")


class BoundedQueue(Generic[T]):
    """Thread-safe bounded queue backed by queue.Queue.

    Producers block when full; consumers block when empty.
    Call complete() to signal no more items will be enqueued.
    """

    def __init__(self, capacity: int) -> None:
        self.capacity = capacity
        self._queue: queue.Queue[T] = queue.Queue(maxsize=capacity)
        self._completed = threading.Event()

    @property
    def count(self) -> int:
        return self._queue.qsize()

    def enqueue(self, item: T, timeout: float | None = None) -> bool:
        """Enqueue an item. Blocks if full. Returns False if completed."""
        if self._completed.is_set():
            return False
        try:
            self._queue.put(item, timeout=timeout)
            return True
        except queue.Full:
            return False

    def try_enqueue(self, item: T) -> bool:
        """Try to enqueue without blocking. Returns False if full or completed."""
        if self._completed.is_set():
            return False
        try:
            self._queue.put_nowait(item)
            return True
        except queue.Full:
            return False

    def dequeue(self, timeout: float = 0.1) -> tuple[bool, T | None]:
        """Dequeue an item. Blocks if empty.

        Returns (True, item) or (False, None) when completed and empty.
        Uses a polling loop so it can detect completion.
        """
        while True:
            try:
                item = self._queue.get(timeout=timeout)
                return True, item
            except queue.Empty:
                if self._completed.is_set() and self._queue.empty():
                    return False, None

    def complete(self) -> None:
        """Signal that no more items will be enqueued."""
        self._completed.set()

    @property
    def is_completed(self) -> bool:
        return self._completed.is_set()

    def items(self) -> "BoundedQueueIterator[T]":
        """Iterator that yields items until the queue is completed and empty."""
        return BoundedQueueIterator(self)


class BoundedQueueIterator(Generic[T]):
    def __init__(self, bounded_queue: BoundedQueue[T]) -> None:
        self._queue = bounded_queue

    def __iter__(self) -> "BoundedQueueIterator[T]":
        return self

    def __next__(self) -> T:
        success, item = self._queue.dequeue()
        if not success:
            raise StopIteration
        return item  # type: ignore[return-value]
