using FluentAssertions;

namespace GameOfLife.Tests;

public class EmptyAndTrivialTests
{
    [Fact]
    public void An_empty_grid_ticks_to_an_empty_grid()
    {
        var grid = new GridBuilder().Build();

        var next = grid.Tick();

        next.LivingCells.Should().BeEmpty();
    }

    [Fact]
    public void A_single_living_cell_dies_after_one_tick()
    {
        var grid = new GridBuilder()
            .WithLivingCellAt(0, 0)
            .Build();

        var next = grid.Tick();

        next.IsAlive(0, 0).Should().BeFalse();
    }
}
