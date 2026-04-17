from .grid_builder import GridBuilder


class TestStillLifes:
    def test_block_is_stable_after_one_tick(self) -> None:
        grid = (
            GridBuilder()
            .with_living_cells_at((0, 0), (0, 1), (1, 0), (1, 1))
            .build()
        )

        next_gen = grid.tick()

        assert next_gen.living_cells() == grid.living_cells()

    def test_block_remains_stable_after_multiple_ticks(self) -> None:
        grid = (
            GridBuilder()
            .with_living_cells_at((0, 0), (0, 1), (1, 0), (1, 1))
            .build()
        )

        after5 = grid.tick().tick().tick().tick().tick()

        assert after5.living_cells() == grid.living_cells()
