using FluentAssertions;
using Xunit;

namespace ZombieSurvivor.Tests;

public class EquipmentTests
{
    [Fact]
    public void New_survivor_can_carry_up_to_five_pieces_of_equipment()
    {
        var survivor = new SurvivorBuilder().Build();

        survivor.EquipmentCapacity.Should().Be(5);
    }

    [Fact]
    public void Survivor_can_hold_up_to_two_items_in_hand()
    {
        var survivor = new SurvivorBuilder()
            .WithEquipment("Bat", "Pistol")
            .Build();

        survivor.InHandCount.Should().Be(2);
    }

    [Fact]
    public void Remaining_equipment_goes_in_reserve()
    {
        var survivor = new SurvivorBuilder()
            .WithEquipment("Bat", "Pistol", "Medkit")
            .Build();

        survivor.InHandCount.Should().Be(2);
        survivor.InReserveCount.Should().Be(1);
    }

    [Fact]
    public void Equipping_a_sixth_item_is_rejected()
    {
        var survivor = new SurvivorBuilder()
            .WithEquipment("Bat", "Pistol", "Medkit", "Axe", "Shield")
            .Build();

        var act = () => survivor.Equip("Grenade");

        act.Should().Throw<EquipmentCapacityExceededException>()
            .WithMessage("Cannot carry more than 5 pieces of equipment.");
    }

    [Fact]
    public void One_wound_reduces_carrying_capacity_to_four()
    {
        var survivor = new SurvivorBuilder().WithWounds(1).Build();

        survivor.EquipmentCapacity.Should().Be(4);
    }

    [Fact]
    public void Wound_with_full_equipment_requires_discarding_one_item()
    {
        var survivor = new SurvivorBuilder()
            .WithEquipment("Bat", "Pistol", "Medkit", "Axe", "Shield")
            .Build();

        survivor.ReceiveWound();

        survivor.NeedsToDiscard.Should().BeTrue();
        survivor.Discard("Shield");
        survivor.NeedsToDiscard.Should().BeFalse();
        survivor.Equipment.Count.Should().Be(4);
    }
}
