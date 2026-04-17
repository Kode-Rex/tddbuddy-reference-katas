using FluentAssertions;
using Xunit;

namespace ZombieSurvivor.Tests;

public class ExperienceAndLevelTests
{
    [Fact]
    public void Killing_a_zombie_awards_one_experience_point()
    {
        var survivor = new SurvivorBuilder().Build();

        survivor.KillZombie();

        survivor.Experience.Should().Be(1);
    }

    [Fact]
    public void Survivor_with_seven_experience_is_level_Yellow()
    {
        var survivor = new SurvivorBuilder().WithZombieKills(7).Build();

        survivor.Level.Should().Be(Level.Yellow);
    }

    [Fact]
    public void Survivor_with_nineteen_experience_is_level_Orange()
    {
        var survivor = new SurvivorBuilder().WithZombieKills(19).Build();

        survivor.Level.Should().Be(Level.Orange);
    }

    [Fact]
    public void Survivor_with_forty_three_experience_is_level_Red()
    {
        var survivor = new SurvivorBuilder().WithZombieKills(43).Build();

        survivor.Level.Should().Be(Level.Red);
    }

    [Fact]
    public void Game_level_matches_the_highest_level_among_living_survivors()
    {
        var (game, _) = new HistoryBuilder()
            .WithSurvivor("Alice")
            .WithSurvivor("Bob")
            .WithZombieKill("Alice", 7)
            .Build();

        game.GameLevel.Should().Be(Level.Yellow);
    }
}
