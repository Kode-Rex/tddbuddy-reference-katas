from multi_threaded_santa import BoundedQueue


class TestBoundedQueue:
    def test_enqueuing_an_item_makes_it_available_for_dequeue(self) -> None:
        q: BoundedQueue[int] = BoundedQueue(10)

        q.enqueue(42)
        success, item = q.dequeue()

        assert success is True
        assert item == 42

    def test_dequeuing_returns_items_in_fifo_order(self) -> None:
        q: BoundedQueue[int] = BoundedQueue(10)

        q.enqueue(1)
        q.enqueue(2)
        q.enqueue(3)

        _, first = q.dequeue()
        _, second = q.dequeue()
        _, third = q.dequeue()

        assert first == 1
        assert second == 2
        assert third == 3

    def test_a_queue_reports_its_current_count(self) -> None:
        q: BoundedQueue[int] = BoundedQueue(10)

        assert q.count == 0

        q.enqueue(1)
        q.enqueue(2)

        assert q.count == 2

        q.dequeue()

        assert q.count == 1

    def test_a_full_queue_rejects_further_enqueues_until_space_is_available(
        self,
    ) -> None:
        q: BoundedQueue[int] = BoundedQueue(2)

        assert q.try_enqueue(1) is True
        assert q.try_enqueue(2) is True
        assert q.try_enqueue(3) is False

        assert q.count == 2
