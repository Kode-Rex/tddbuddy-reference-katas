using FluentAssertions;
using Xunit;

namespace JellyVsTower.Tests;

public class JellyVsTowerTests
{
    // ── Jelly Creation and Health ──────────────────────────────

    [Fact]
    public void Jelly_starts_alive_with_the_given_health()
    {
        var jelly = new JellyBuilder().WithHealth(20).Build();

        jelly.Health.Should().Be(20);
        jelly.IsAlive.Should().BeTrue();
    }

    [Fact]
    public void Jelly_with_zero_health_is_rejected()
    {
        var act = () => new JellyBuilder().WithHealth(0).Build();

        act.Should().Throw<InvalidHealthException>()
            .WithMessage("Health must be strictly positive, got 0");
    }

    [Fact]
    public void Jelly_with_negative_health_is_rejected()
    {
        var act = () => new JellyBuilder().WithHealth(-5).Build();

        act.Should().Throw<InvalidHealthException>()
            .WithMessage("Health must be strictly positive, got -5");
    }

    [Fact]
    public void Jelly_dies_when_health_reaches_zero()
    {
        var jelly = new JellyBuilder().WithHealth(5).Build();

        jelly.TakeDamage(5);

        jelly.IsAlive.Should().BeFalse();
        jelly.Health.Should().Be(0);
    }

    [Fact]
    public void Jelly_dies_when_health_drops_below_zero()
    {
        var jelly = new JellyBuilder().WithHealth(3).Build();

        jelly.TakeDamage(10);

        jelly.IsAlive.Should().BeFalse();
        jelly.Health.Should().Be(0);
    }

    // ── Tower Creation and Validation ─────────────────────────

    [Fact]
    public void Tower_is_created_with_a_color_and_level()
    {
        var tower = new TowerBuilder().WithColor(ColorType.Red).WithLevel(3).Build();

        tower.Color.Should().Be(ColorType.Red);
        tower.Level.Should().Be(3);
    }

    [Fact]
    public void Tower_with_level_below_1_is_rejected()
    {
        var act = () => new TowerBuilder().WithLevel(0).Build();

        act.Should().Throw<InvalidLevelException>()
            .WithMessage("Tower level must be between 1 and 4, got 0");
    }

    [Fact]
    public void Tower_with_level_above_4_is_rejected()
    {
        var act = () => new TowerBuilder().WithLevel(5).Build();

        act.Should().Throw<InvalidLevelException>()
            .WithMessage("Tower level must be between 1 and 4, got 5");
    }

    // ── Damage Lookup — Blue Tower ────────────────────────────

    [Fact]
    public void Blue_tower_level_1_deals_2_to_5_damage_to_a_blue_jelly()
    {
        var tower = new TowerBuilder().WithColor(ColorType.Blue).WithLevel(1).Build();
        var jelly = new JellyBuilder().WithColor(ColorType.Blue).WithHealth(100).Build();

        var minDamage = DamageTable.CalculateDamage(tower, jelly, new FixedRandomSource(2));
        var maxDamage = DamageTable.CalculateDamage(tower, jelly, new FixedRandomSource(5));

        minDamage.Should().Be(2);
        maxDamage.Should().Be(5);
    }

    [Fact]
    public void Blue_tower_level_1_deals_0_damage_to_a_red_jelly()
    {
        var tower = new TowerBuilder().WithColor(ColorType.Blue).WithLevel(1).Build();
        var jelly = new JellyBuilder().WithColor(ColorType.Red).WithHealth(100).Build();

        var damage = DamageTable.CalculateDamage(tower, jelly, new FixedRandomSource(0));

        damage.Should().Be(0);
    }

    [Fact]
    public void Blue_tower_level_4_deals_12_to_15_damage_to_a_blue_jelly()
    {
        var tower = new TowerBuilder().WithColor(ColorType.Blue).WithLevel(4).Build();
        var jelly = new JellyBuilder().WithColor(ColorType.Blue).WithHealth(100).Build();

        var minDamage = DamageTable.CalculateDamage(tower, jelly, new FixedRandomSource(12));
        var maxDamage = DamageTable.CalculateDamage(tower, jelly, new FixedRandomSource(15));

        minDamage.Should().Be(12);
        maxDamage.Should().Be(15);
    }

    [Fact]
    public void Blue_tower_level_2_deals_1_damage_to_a_red_jelly()
    {
        var tower = new TowerBuilder().WithColor(ColorType.Blue).WithLevel(2).Build();
        var jelly = new JellyBuilder().WithColor(ColorType.Red).WithHealth(100).Build();

        var damage = DamageTable.CalculateDamage(tower, jelly, new FixedRandomSource(1));

        damage.Should().Be(1);
    }

    // ── Damage Lookup — Red Tower ─────────────────────────────

    [Fact]
    public void Red_tower_level_3_deals_9_to_12_damage_to_a_red_jelly()
    {
        var tower = new TowerBuilder().WithColor(ColorType.Red).WithLevel(3).Build();
        var jelly = new JellyBuilder().WithColor(ColorType.Red).WithHealth(100).Build();

        var minDamage = DamageTable.CalculateDamage(tower, jelly, new FixedRandomSource(9));
        var maxDamage = DamageTable.CalculateDamage(tower, jelly, new FixedRandomSource(12));

        minDamage.Should().Be(9);
        maxDamage.Should().Be(12);
    }

    [Fact]
    public void Red_tower_level_2_deals_1_damage_to_a_blue_jelly()
    {
        var tower = new TowerBuilder().WithColor(ColorType.Red).WithLevel(2).Build();
        var jelly = new JellyBuilder().WithColor(ColorType.Blue).WithHealth(100).Build();

        var damage = DamageTable.CalculateDamage(tower, jelly, new FixedRandomSource(1));

        damage.Should().Be(1);
    }

    [Fact]
    public void Red_tower_level_1_deals_0_damage_to_a_blue_jelly()
    {
        var tower = new TowerBuilder().WithColor(ColorType.Red).WithLevel(1).Build();
        var jelly = new JellyBuilder().WithColor(ColorType.Blue).WithHealth(100).Build();

        var damage = DamageTable.CalculateDamage(tower, jelly, new FixedRandomSource(0));

        damage.Should().Be(0);
    }

    // ── Damage Lookup — BlueRed Tower ─────────────────────────

    [Fact]
    public void BlueRed_tower_level_4_deals_6_to_8_damage_to_a_blue_jelly()
    {
        var tower = new TowerBuilder().WithColor(ColorType.BlueRed).WithLevel(4).Build();
        var jelly = new JellyBuilder().WithColor(ColorType.Blue).WithHealth(100).Build();

        var minDamage = DamageTable.CalculateDamage(tower, jelly, new FixedRandomSource(6));
        var maxDamage = DamageTable.CalculateDamage(tower, jelly, new FixedRandomSource(8));

        minDamage.Should().Be(6);
        maxDamage.Should().Be(8);
    }

    [Fact]
    public void BlueRed_tower_level_4_deals_6_to_8_damage_to_a_red_jelly()
    {
        var tower = new TowerBuilder().WithColor(ColorType.BlueRed).WithLevel(4).Build();
        var jelly = new JellyBuilder().WithColor(ColorType.Red).WithHealth(100).Build();

        var minDamage = DamageTable.CalculateDamage(tower, jelly, new FixedRandomSource(6));
        var maxDamage = DamageTable.CalculateDamage(tower, jelly, new FixedRandomSource(8));

        minDamage.Should().Be(6);
        maxDamage.Should().Be(8);
    }

    [Fact]
    public void BlueRed_tower_level_1_deals_2_damage_to_a_blue_jelly()
    {
        var tower = new TowerBuilder().WithColor(ColorType.BlueRed).WithLevel(1).Build();
        var jelly = new JellyBuilder().WithColor(ColorType.Blue).WithHealth(100).Build();

        var damage = DamageTable.CalculateDamage(tower, jelly, new FixedRandomSource(2));

        damage.Should().Be(2);
    }

    // ── BlueRed Jelly — Takes Higher Column ───────────────────

    [Fact]
    public void BlueRed_jelly_takes_the_higher_of_blue_and_red_column_damage()
    {
        // Blue tower L1: vs Blue = 2–5, vs Red = 0
        // BlueRed jelly → max(blue-col, red-col) → uses blue column
        var tower = new TowerBuilder().WithColor(ColorType.Blue).WithLevel(1).Build();
        var jelly = new JellyBuilder().WithColor(ColorType.BlueRed).WithHealth(100).Build();

        var damage = DamageTable.CalculateDamage(tower, jelly, new FixedRandomSource(3));

        damage.Should().Be(3);
    }

    [Fact]
    public void BlueRed_jelly_hit_by_BlueRed_tower_uses_both_columns_and_takes_the_higher()
    {
        // BlueRed tower L2: vs Blue = 2–4, vs Red = 2–4 — symmetric, both resolve the same
        var tower = new TowerBuilder().WithColor(ColorType.BlueRed).WithLevel(2).Build();
        var jelly = new JellyBuilder().WithColor(ColorType.BlueRed).WithHealth(100).Build();

        var damage = DamageTable.CalculateDamage(tower, jelly, new FixedRandomSource(3));

        damage.Should().Be(3);
    }

    // ── Combat Flow ───────────────────────────────────────────

    [Fact]
    public void Tower_attacks_the_first_alive_jelly_and_produces_a_combat_log()
    {
        var tower = new TowerBuilder().WithId("T1").WithColor(ColorType.Blue).WithLevel(1).Build();
        var jelly = new JellyBuilder().WithId("J1").WithColor(ColorType.Blue).WithHealth(20).Build();
        var arena = new Arena(new[] { tower }, new[] { jelly }, new FixedRandomSource(3));

        var logs = arena.ExecuteRound();

        logs.Should().ContainSingle();
        logs[0].TowerId.Should().Be("T1");
        logs[0].JellyId.Should().Be("J1");
        logs[0].Damage.Should().Be(3);
        jelly.Health.Should().Be(17);
    }

    [Fact]
    public void Dead_jellies_are_skipped_tower_attacks_the_next_alive_jelly()
    {
        var tower = new TowerBuilder().WithId("T1").WithColor(ColorType.Blue).WithLevel(4).Build();
        var deadJelly = new JellyBuilder().WithId("J1").WithColor(ColorType.Blue).WithHealth(1).Build();
        deadJelly.TakeDamage(1);
        var aliveJelly = new JellyBuilder().WithId("J2").WithColor(ColorType.Blue).WithHealth(20).Build();
        var arena = new Arena(new[] { tower }, new[] { deadJelly, aliveJelly }, new FixedRandomSource(12));

        var logs = arena.ExecuteRound();

        logs.Should().ContainSingle();
        logs[0].JellyId.Should().Be("J2");
    }

    [Fact]
    public void Tower_attack_does_nothing_when_no_alive_jellies_remain()
    {
        var tower = new TowerBuilder().WithColor(ColorType.Blue).WithLevel(1).Build();
        var jelly = new JellyBuilder().WithHealth(1).Build();
        jelly.TakeDamage(1);
        var arena = new Arena(new[] { tower }, new[] { jelly }, new FixedRandomSource(3));

        var logs = arena.ExecuteRound();

        logs.Should().BeEmpty();
    }

    [Fact]
    public void Multiple_towers_each_attack_in_a_single_round()
    {
        var tower1 = new TowerBuilder().WithId("T1").WithColor(ColorType.Blue).WithLevel(1).Build();
        var tower2 = new TowerBuilder().WithId("T2").WithColor(ColorType.Red).WithLevel(1).Build();
        var jelly = new JellyBuilder().WithId("J1").WithColor(ColorType.Blue).WithHealth(100).Build();
        var arena = new Arena(new[] { tower1, tower2 }, new[] { jelly }, new FixedRandomSource(3));

        var logs = arena.ExecuteRound();

        logs.Should().HaveCount(2);
        logs[0].TowerId.Should().Be("T1");
        logs[1].TowerId.Should().Be("T2");
    }

    [Fact]
    public void Jelly_killed_during_a_round_is_removed_before_the_next_tower_attacks()
    {
        var tower1 = new TowerBuilder().WithId("T1").WithColor(ColorType.Blue).WithLevel(4).Build();
        var tower2 = new TowerBuilder().WithId("T2").WithColor(ColorType.Blue).WithLevel(1).Build();
        var jelly1 = new JellyBuilder().WithId("J1").WithColor(ColorType.Blue).WithHealth(1).Build();
        var jelly2 = new JellyBuilder().WithId("J2").WithColor(ColorType.Blue).WithHealth(20).Build();
        var arena = new Arena(new[] { tower1, tower2 }, new[] { jelly1, jelly2 }, new FixedRandomSource(12));

        var logs = arena.ExecuteRound();

        logs.Should().HaveCount(2);
        logs[0].JellyId.Should().Be("J1");
        logs[1].JellyId.Should().Be("J2");
        jelly1.IsAlive.Should().BeFalse();
    }
}
