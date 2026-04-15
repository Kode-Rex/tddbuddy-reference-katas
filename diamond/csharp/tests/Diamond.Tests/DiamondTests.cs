using FluentAssertions;
using Xunit;

namespace Diamond.Tests;

public class DiamondTests
{
    [Fact]
    public void A_is_a_single_letter_diamond()
    {
        Diamond.Print('A').Should().Be("A");
    }

    [Fact]
    public void B_renders_three_rows_with_a_single_inner_space()
    {
        Diamond.Print('B').Should().Be(" A\nB B\n A");
    }

    [Fact]
    public void C_renders_five_rows_and_three_inner_spaces_on_the_widest_row()
    {
        Diamond.Print('C').Should().Be("  A\n B B\nC   C\n B B\n  A");
    }

    [Fact]
    public void D_renders_seven_rows_with_a_five_space_widest_row()
    {
        var expected = string.Join("\n",
            "   A",
            "  B B",
            " C   C",
            "D     D",
            " C   C",
            "  B B",
            "   A");
        Diamond.Print('D').Should().Be(expected);
    }

    [Fact]
    public void E_renders_nine_rows_with_a_seven_space_widest_row()
    {
        var expected = string.Join("\n",
            "    A",
            "   B B",
            "  C   C",
            " D     D",
            "E       E",
            " D     D",
            "  C   C",
            "   B B",
            "    A");
        Diamond.Print('E').Should().Be(expected);
    }

    [Fact]
    public void Z_renders_a_full_fifty_one_row_diamond()
    {
        var output = Diamond.Print('Z');
        var rows = output.Split('\n');
        rows.Should().HaveCount(51);
        rows[0].Should().Be(new string(' ', 25) + "A");
        rows[25].Should().Be("Z" + new string(' ', 49) + "Z");
        rows[50].Should().Be(new string(' ', 25) + "A");
    }

    [Fact]
    public void Lowercase_input_is_normalized_to_uppercase()
    {
        Diamond.Print('c').Should().Be(Diamond.Print('C'));
    }

    [Fact]
    public void Top_half_mirrors_bottom_half()
    {
        var rows = Diamond.Print('F').Split('\n');
        var n = rows.Length / 2;
        for (var r = 0; r <= n; r++)
        {
            rows[r].Should().Be(rows[rows.Length - 1 - r]);
        }
    }

    [Fact]
    public void No_row_has_trailing_whitespace()
    {
        foreach (var row in Diamond.Print('G').Split('\n'))
        {
            row.Should().Be(row.TrimEnd());
        }
    }

    [Fact]
    public void Non_letter_input_is_rejected()
    {
        var act = () => Diamond.Print('1');
        act.Should().Throw<ArgumentException>()
            .WithMessage("letter must be a single A-Z character*");
    }
}
