from multi_threaded_santa import calculate_cookies


class TestCostCalculation:
    def test_cost_is_zero_when_no_elves_are_used(self) -> None:
        assert calculate_cookies(0, 100) == 0

    def test_cost_equals_elves_multiplied_by_elapsed_seconds(self) -> None:
        assert calculate_cookies(5, 10) == 50

    def test_more_elves_with_shorter_time_can_cost_the_same_as_fewer_elves_with_longer_time(
        self,
    ) -> None:
        cost_a = calculate_cookies(10, 5)
        cost_b = calculate_cookies(5, 10)

        assert cost_a == cost_b
