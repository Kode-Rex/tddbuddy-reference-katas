import threading
from typing import Sequence

from multi_threaded_santa.bounded_queue import BoundedQueue
from multi_threaded_santa.present import Present


class Pipeline:
    """Four-stage present pipeline: Make -> Wrap -> Load -> Deliver.

    Supports sequential (single-thread) and concurrent (multi-worker) execution.
    The sleigh lock ensures mutual exclusion between loading and delivering.
    """

    def __init__(
        self,
        made_capacity: int = 1000,
        wrapped_capacity: int = 2000,
        loaded_capacity: int = 5000,
    ) -> None:
        self.made_queue: BoundedQueue[Present] = BoundedQueue(made_capacity)
        self.wrapped_queue: BoundedQueue[Present] = BoundedQueue(wrapped_capacity)
        self.loaded_queue: BoundedQueue[Present] = BoundedQueue(loaded_capacity)
        self._delivered: list[Present] = []
        self._sleigh_lock = threading.Lock()
        self._delivered_lock = threading.Lock()

    @property
    def delivered(self) -> list[Present]:
        with self._delivered_lock:
            return list(self._delivered)

    def process_sequentially(self, presents: Sequence[Present]) -> None:
        """Process all presents through the pipeline on a single thread."""
        for present in presents:
            present.make()
            self.made_queue.enqueue(present)
        self.made_queue.complete()

        for present in self.made_queue.items():
            present.wrap()
            self.wrapped_queue.enqueue(present)
        self.wrapped_queue.complete()

        for present in self.wrapped_queue.items():
            present.load()
            self.loaded_queue.enqueue(present)
        self.loaded_queue.complete()

        for present in self.loaded_queue.items():
            present.deliver()
            with self._delivered_lock:
                self._delivered.append(present)

    def process_concurrently(
        self,
        presents: Sequence[Present],
        make_workers: int = 1,
        wrap_workers: int = 1,
        load_workers: int = 1,
    ) -> None:
        """Process presents concurrently with configurable worker counts.

        Delivery always uses one worker (sleigh constraint).
        """
        # Feed queue distributes presents to makers
        feed_queue: BoundedQueue[Present] = BoundedQueue(
            len(presents) if presents else 1
        )
        for p in presents:
            feed_queue.enqueue(p)
        feed_queue.complete()

        # Stage 1: Make
        maker_threads = self._launch_workers(
            make_workers,
            lambda: self._make_worker(feed_queue),
        )

        # Stage 2: Wrap
        wrap_threads = self._launch_workers(
            wrap_workers,
            lambda: self._wrap_worker(),
        )

        # Stage 3: Load (acquires sleigh lock)
        load_threads = self._launch_workers(
            load_workers,
            lambda: self._load_worker(),
        )

        # Stage 4: Deliver (single worker)
        deliver_thread = threading.Thread(target=self._deliver_worker, daemon=True)
        deliver_thread.start()

        # Cascade completions
        for t in maker_threads:
            t.join()
        self.made_queue.complete()

        for t in wrap_threads:
            t.join()
        self.wrapped_queue.complete()

        for t in load_threads:
            t.join()
        self.loaded_queue.complete()

        deliver_thread.join()

    @staticmethod
    def total_elves(
        make_workers: int, wrap_workers: int, load_workers: int
    ) -> int:
        return make_workers + wrap_workers + load_workers + 1  # +1 for delivery

    def _make_worker(self, feed_queue: BoundedQueue[Present]) -> None:
        for present in feed_queue.items():
            present.make()
            self.made_queue.enqueue(present)

    def _wrap_worker(self) -> None:
        for present in self.made_queue.items():
            present.wrap()
            self.wrapped_queue.enqueue(present)

    def _load_worker(self) -> None:
        for present in self.wrapped_queue.items():
            with self._sleigh_lock:
                present.load()
                self.loaded_queue.enqueue(present)

    def _deliver_worker(self) -> None:
        for present in self.loaded_queue.items():
            with self._sleigh_lock:
                present.deliver()
                with self._delivered_lock:
                    self._delivered.append(present)

    @staticmethod
    def _launch_workers(
        count: int, target: object
    ) -> list[threading.Thread]:
        threads = []
        for _ in range(count):
            t = threading.Thread(target=target, daemon=True)  # type: ignore[arg-type]
            t.start()
            threads.append(t)
        return threads
