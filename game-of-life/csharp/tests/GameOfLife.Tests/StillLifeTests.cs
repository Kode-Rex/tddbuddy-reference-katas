using FluentAssertions;

namespace GameOfLife.Tests;

public class StillLifeTests
{
    [Fact]
    public void Block_is_stable_after_one_tick()
    {
        var grid = new GridBuilder()
            .WithLivingCellsAt((0, 0), (0, 1), (1, 0), (1, 1))
            .Build();

        var next = grid.Tick();

        next.LivingCells.Should().BeEquivalentTo(grid.LivingCells);
    }

    [Fact]
    public void Block_remains_stable_after_multiple_ticks()
    {
        var grid = new GridBuilder()
            .WithLivingCellsAt((0, 0), (0, 1), (1, 0), (1, 1))
            .Build();

        var after5 = grid.Tick().Tick().Tick().Tick().Tick();

        after5.LivingCells.Should().BeEquivalentTo(grid.LivingCells);
    }
}
