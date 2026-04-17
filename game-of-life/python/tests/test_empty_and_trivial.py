from .grid_builder import GridBuilder


class TestEmptyAndTrivial:
    def test_an_empty_grid_ticks_to_an_empty_grid(self) -> None:
        grid = GridBuilder().build()

        next_gen = grid.tick()

        assert len(next_gen.living_cells()) == 0

    def test_a_single_living_cell_dies_after_one_tick(self) -> None:
        grid = GridBuilder().with_living_cell_at(0, 0).build()

        next_gen = grid.tick()

        assert not next_gen.is_alive(0, 0)
