from multi_threaded_santa import Present, PresentState


class TestPresentLifecycle:
    def test_a_new_present_starts_in_the_unmade_state(self) -> None:
        present = Present(1)

        assert present.state == PresentState.UNMADE

    def test_making_a_present_transitions_it_to_the_made_state(self) -> None:
        present = Present(1)

        present.make()

        assert present.state == PresentState.MADE

    def test_wrapping_a_made_present_transitions_it_to_the_wrapped_state(self) -> None:
        present = Present(1)
        present.make()

        present.wrap()

        assert present.state == PresentState.WRAPPED

    def test_loading_a_wrapped_present_transitions_it_to_the_loaded_state(self) -> None:
        present = Present(1)
        present.make()
        present.wrap()

        present.load()

        assert present.state == PresentState.LOADED

    def test_delivering_a_loaded_present_transitions_it_to_the_delivered_state(
        self,
    ) -> None:
        present = Present(1)
        present.make()
        present.wrap()
        present.load()

        present.deliver()

        assert present.state == PresentState.DELIVERED
