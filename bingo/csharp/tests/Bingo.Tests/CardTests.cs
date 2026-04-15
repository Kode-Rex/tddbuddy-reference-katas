using FluentAssertions;
using Xunit;

namespace Bingo.Tests;

public class CardTests
{
    // A complete 5x5 card used by scenarios that need every cell populated.
    // Column ranges are honoured for realism even though the builder does
    // not enforce them: B 1-15, I 16-30, N 31-45 (free at (2,2)), G 46-60, O 61-75.
    private static CardBuilder AFullCard() =>
        new CardBuilder()
            .WithNumberAt(0, 0, 3).WithNumberAt(0, 1, 17).WithNumberAt(0, 2, 33).WithNumberAt(0, 3, 48).WithNumberAt(0, 4, 62)
            .WithNumberAt(1, 0, 8).WithNumberAt(1, 1, 22).WithNumberAt(1, 2, 38).WithNumberAt(1, 3, 52).WithNumberAt(1, 4, 67)
            .WithNumberAt(2, 0, 11).WithNumberAt(2, 1, 27)                         .WithNumberAt(2, 3, 55).WithNumberAt(2, 4, 70)
            .WithNumberAt(3, 0, 4).WithNumberAt(3, 1, 19).WithNumberAt(3, 2, 41).WithNumberAt(3, 3, 58).WithNumberAt(3, 4, 73)
            .WithNumberAt(4, 0, 15).WithNumberAt(4, 1, 30).WithNumberAt(4, 2, 45).WithNumberAt(4, 3, 60).WithNumberAt(4, 4, 75);

    [Fact]
    public void Blank_card_reports_no_win_and_no_marks()
    {
        var card = AFullCard().Build();

        card.HasWon().Should().BeFalse();
        card.WinningPattern().Should().Be(WinPattern.None);
        for (var r = 0; r < 5; r++)
            for (var c = 0; c < 5; c++)
                if (!(r == 2 && c == 2))
                    card.IsMarked(r, c).Should().BeFalse();
    }

    [Fact]
    public void Free_space_starts_marked()
    {
        var card = AFullCard().Build();

        card.IsMarked(2, 2).Should().BeTrue();
    }

    [Fact]
    public void Marking_a_number_that_is_on_the_card_marks_the_matching_cell()
    {
        var card = AFullCard().Build();

        card.Mark(3);

        card.IsMarked(0, 0).Should().BeTrue();
        card.IsMarked(0, 1).Should().BeFalse();
    }

    [Fact]
    public void Marking_a_number_not_on_the_card_is_a_silent_no_op()
    {
        var card = AFullCard().Build();

        var act = () => card.Mark(42); // 42 is in the I-column range but not placed on this card

        act.Should().NotThrow();
        for (var r = 0; r < 5; r++)
            for (var c = 0; c < 5; c++)
                if (!(r == 2 && c == 2))
                    card.IsMarked(r, c).Should().BeFalse();
    }

    [Fact]
    public void Marking_a_number_outside_1_to_75_raises()
    {
        var card = AFullCard().Build();

        var below = () => card.Mark(0);
        var above = () => card.Mark(76);

        below.Should().Throw<NumberOutOfRangeException>()
            .WithMessage("called number must be between 1 and 75*");
        above.Should().Throw<NumberOutOfRangeException>()
            .WithMessage("called number must be between 1 and 75*");
    }

    [Fact]
    public void Completing_row_0_wins_on_that_row()
    {
        var card = AFullCard().Build();

        card.Mark(3); card.Mark(17); card.Mark(33); card.Mark(48); card.Mark(62);

        card.HasWon().Should().BeTrue();
        card.WinningPattern().Should().Be(WinPattern.Row(0));
    }

    [Fact]
    public void Completing_column_4_wins_on_that_column()
    {
        var card = AFullCard().Build();

        card.Mark(62); card.Mark(67); card.Mark(70); card.Mark(73); card.Mark(75);

        card.WinningPattern().Should().Be(WinPattern.Column(4));
    }

    [Fact]
    public void Completing_the_main_diagonal_wins_on_DiagonalMain()
    {
        var card = AFullCard().Build();

        card.Mark(3);  // (0,0)
        card.Mark(22); // (1,1)
        // (2,2) is the free space, already marked
        card.Mark(58); // (3,3)
        card.Mark(75); // (4,4)

        card.WinningPattern().Should().Be(WinPattern.DiagonalMain);
    }

    [Fact]
    public void Completing_the_anti_diagonal_wins_on_DiagonalAnti()
    {
        var card = AFullCard().Build();

        card.Mark(62); // (0,4)
        card.Mark(52); // (1,3)
        // (2,2) is the free space, already marked
        card.Mark(19); // (3,1)
        card.Mark(15); // (4,0)

        card.WinningPattern().Should().Be(WinPattern.DiagonalAnti);
    }

    [Fact]
    public void Four_marks_in_a_row_is_not_a_win()
    {
        var card = AFullCard().Build();

        card.Mark(3); card.Mark(17); card.Mark(33); card.Mark(48);
        // row 0 needs 62 to win; leave it unmarked

        card.HasWon().Should().BeFalse();
        card.WinningPattern().Should().Be(WinPattern.None);
    }

    [Fact]
    public void Winning_pattern_scan_order_is_rows_then_columns_then_diagonals()
    {
        // Row 0 fully marked AND column 0 fully marked — the scan should report Row(0) first.
        var card = new CardBuilder()
            .WithNumberAt(0, 0, 3).WithNumberAt(0, 1, 17).WithNumberAt(0, 2, 33).WithNumberAt(0, 3, 48).WithNumberAt(0, 4, 62)
            .WithNumberAt(1, 0, 8)
            .WithNumberAt(2, 0, 11)
            .WithNumberAt(3, 0, 4)
            .WithNumberAt(4, 0, 15)
            .WithMarkAt(0, 0).WithMarkAt(0, 1).WithMarkAt(0, 2).WithMarkAt(0, 3).WithMarkAt(0, 4)
            .WithMarkAt(1, 0).WithMarkAt(2, 0).WithMarkAt(3, 0).WithMarkAt(4, 0)
            .Build();

        card.WinningPattern().Should().Be(WinPattern.Row(0));
    }

    [Fact]
    public void CardBuilder_produces_the_card_the_test_literal_describes()
    {
        var card = new CardBuilder().WithNumberAt(0, 0, 3).Build();

        card.NumberAt(0, 0).Should().Be(3);
        card.NumberAt(2, 2).Should().BeNull();
        card.IsMarked(2, 2).Should().BeTrue();
        card.IsMarked(0, 0).Should().BeFalse();

        card.Mark(3);
        card.IsMarked(0, 0).Should().BeTrue();
    }
}
