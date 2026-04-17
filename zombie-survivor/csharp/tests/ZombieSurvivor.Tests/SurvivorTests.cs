using FluentAssertions;
using Xunit;

namespace ZombieSurvivor.Tests;

public class SurvivorTests
{
    [Fact]
    public void New_survivor_has_zero_wounds()
    {
        var survivor = new SurvivorBuilder().Build();

        survivor.Wounds.Should().Be(0);
    }

    [Fact]
    public void New_survivor_has_three_actions_per_turn()
    {
        var survivor = new SurvivorBuilder().Build();

        survivor.ActionsPerTurn.Should().Be(3);
    }

    [Fact]
    public void New_survivor_is_alive()
    {
        var survivor = new SurvivorBuilder().Build();

        survivor.IsAlive.Should().BeTrue();
    }

    [Fact]
    public void New_survivor_starts_at_level_Blue()
    {
        var survivor = new SurvivorBuilder().Build();

        survivor.Level.Should().Be(Level.Blue);
    }

    [Fact]
    public void Receiving_a_wound_leaves_the_survivor_alive_with_one_wound()
    {
        var survivor = new SurvivorBuilder().Build();

        survivor.ReceiveWound();

        survivor.Wounds.Should().Be(1);
        survivor.IsAlive.Should().BeTrue();
    }

    [Fact]
    public void Receiving_a_second_wound_kills_the_survivor()
    {
        var survivor = new SurvivorBuilder().WithWounds(1).Build();

        survivor.ReceiveWound();

        survivor.IsAlive.Should().BeFalse();
        survivor.Wounds.Should().Be(2);
    }

    [Fact]
    public void Wounding_a_dead_survivor_has_no_effect()
    {
        var survivor = new SurvivorBuilder().WithWounds(2).Build();

        survivor.ReceiveWound();

        survivor.Wounds.Should().Be(2);
    }
}
