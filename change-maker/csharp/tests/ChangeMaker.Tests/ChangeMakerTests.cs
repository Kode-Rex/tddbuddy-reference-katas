using FluentAssertions;
using Xunit;

namespace ChangeMaker.Tests;

public class ChangeMakerTests
{
    private static readonly int[] UsCoins = { 25, 10, 5, 1 };
    private static readonly int[] UkCoins = { 50, 20, 10, 5, 2, 1 };
    private static readonly int[] NorwayCoins = { 20, 10, 5, 1 };

    [Fact]
    public void Zero_amount_returns_no_coins()
    {
        ChangeMaker.MakeChange(0, UsCoins).Should().BeEmpty();
    }

    [Fact]
    public void US_one_cent_is_a_single_penny()
    {
        ChangeMaker.MakeChange(1, UsCoins).Should().Equal(1);
    }

    [Fact]
    public void US_five_cents_is_a_single_nickel()
    {
        ChangeMaker.MakeChange(5, UsCoins).Should().Equal(5);
    }

    [Fact]
    public void US_twenty_five_cents_is_a_single_quarter()
    {
        ChangeMaker.MakeChange(25, UsCoins).Should().Equal(25);
    }

    [Fact]
    public void US_thirty_cents_is_a_quarter_and_a_nickel()
    {
        ChangeMaker.MakeChange(30, UsCoins).Should().Equal(25, 5);
    }

    [Fact]
    public void US_forty_one_cents_is_a_quarter_a_dime_a_nickel_and_a_penny()
    {
        ChangeMaker.MakeChange(41, UsCoins).Should().Equal(25, 10, 5, 1);
    }

    [Fact]
    public void US_sixty_six_cents_is_two_quarters_a_dime_a_nickel_and_a_penny()
    {
        ChangeMaker.MakeChange(66, UsCoins).Should().Equal(25, 25, 10, 5, 1);
    }

    [Fact]
    public void UK_forty_three_pence_is_twenty_twenty_two_one()
    {
        ChangeMaker.MakeChange(43, UkCoins).Should().Equal(20, 20, 2, 1);
    }

    [Fact]
    public void UK_eighty_eight_pence_is_one_of_each_british_coin()
    {
        ChangeMaker.MakeChange(88, UkCoins).Should().Equal(50, 20, 10, 5, 2, 1);
    }

    [Fact]
    public void Norway_thirty_seven_ore_is_twenty_ten_five_one_one()
    {
        ChangeMaker.MakeChange(37, NorwayCoins).Should().Equal(20, 10, 5, 1, 1);
    }

    [Fact]
    public void Norway_forty_ore_is_two_twenty_ore_coins()
    {
        ChangeMaker.MakeChange(40, NorwayCoins).Should().Equal(20, 20);
    }
}
