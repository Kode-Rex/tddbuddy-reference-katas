using FluentAssertions;

namespace GameOfLife.Tests;

public class SpaceshipTests
{
    [Fact]
    public void Glider_translates_one_cell_down_and_right_after_four_ticks()
    {
        // Standard glider at origin:
        //   .X.
        //   ..X
        //   XXX
        var glider = new GridBuilder()
            .WithLivingCellsAt((0, 1), (1, 2), (2, 0), (2, 1), (2, 2))
            .Build();

        var after4 = glider.Tick().Tick().Tick().Tick();

        // Should be the same shape, shifted (1,1):
        var expected = new Cell[]
        {
            new(1, 2), new(2, 3), new(3, 1), new(3, 2), new(3, 3)
        };
        after4.LivingCells.Should().BeEquivalentTo(expected);
    }
}
