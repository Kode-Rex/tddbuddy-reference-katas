using FluentAssertions;
using Xunit;

namespace ZombieSurvivor.Tests;

public class SkillTests
{
    [Fact]
    public void Reaching_Yellow_unlocks_plus_one_action_as_the_mandatory_first_skill()
    {
        var (game, _) = new HistoryBuilder()
            .WithSurvivor("Alice")
            .WithZombieKill("Alice", 7)
            .Build();

        var alice = game.Survivors.First(s => s.Name == "Alice");
        alice.Skills.Should().Contain(Skill.PlusOneAction);
    }

    [Fact]
    public void Plus_one_action_skill_increases_actions_to_four()
    {
        var (game, _) = new HistoryBuilder()
            .WithSurvivor("Alice")
            .WithZombieKill("Alice", 7)
            .Build();

        var alice = game.Survivors.First(s => s.Name == "Alice");
        alice.ActionsPerTurn.Should().Be(4);
    }

    [Fact]
    public void Hoard_skill_increases_equipment_capacity_by_one()
    {
        var survivor = new SurvivorBuilder().Build();

        survivor.UnlockSkill(Skill.Hoard);

        survivor.EquipmentCapacity.Should().Be(6);
    }
}
