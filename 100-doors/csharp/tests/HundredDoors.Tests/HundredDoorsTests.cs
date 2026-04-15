using FluentAssertions;
using Xunit;

namespace HundredDoors.Tests;

public class HundredDoorsTests
{
    [Fact]
    public void Zero_doors_leaves_no_doors_open()
    {
        HundredDoors.OpenDoors(0).Should().BeEmpty();
    }

    [Fact]
    public void One_door_is_open_after_the_single_pass()
    {
        HundredDoors.OpenDoors(1).Should().Equal(1);
    }

    [Fact]
    public void Ten_doors_leaves_the_perfect_squares_one_four_and_nine_open()
    {
        HundredDoors.OpenDoors(10).Should().Equal(1, 4, 9);
    }

    [Fact]
    public void One_hundred_doors_leaves_the_ten_perfect_squares_open()
    {
        HundredDoors.OpenDoors(100).Should().Equal(1, 4, 9, 16, 25, 36, 49, 64, 81, 100);
    }
}
