using FluentAssertions;
using Xunit;

namespace EndOfLineTrim.Tests;

public class EndOfLineTrimTests
{
    [Fact]
    public void No_trailing_whitespace_is_returned_unchanged()
    {
        EndOfLineTrim.Trim("abc").Should().Be("abc");
    }

    [Fact]
    public void Trailing_space_is_removed()
    {
        EndOfLineTrim.Trim("abc ").Should().Be("abc");
    }

    [Fact]
    public void Trailing_tab_is_removed()
    {
        EndOfLineTrim.Trim("abc\t").Should().Be("abc");
    }

    [Fact]
    public void Leading_whitespace_is_preserved()
    {
        EndOfLineTrim.Trim(" abc").Should().Be(" abc");
    }

    [Fact]
    public void Trailing_whitespace_is_removed_on_each_CRLF_line()
    {
        EndOfLineTrim.Trim("ab\r\n cd \r\n").Should().Be("ab\r\n cd\r\n");
    }

    [Fact]
    public void A_lone_CRLF_is_returned_unchanged()
    {
        EndOfLineTrim.Trim("\r\n").Should().Be("\r\n");
    }

    [Fact]
    public void Trailing_whitespace_is_removed_on_each_LF_line()
    {
        EndOfLineTrim.Trim("ab\n cd \n").Should().Be("ab\n cd\n");
    }

    [Fact]
    public void Whitespace_only_line_collapses_but_keeps_its_terminator()
    {
        EndOfLineTrim.Trim("  \n").Should().Be("\n");
    }

    [Fact]
    public void An_empty_string_returns_an_empty_string()
    {
        EndOfLineTrim.Trim("").Should().Be("");
    }

    [Fact]
    public void Mixed_line_endings_are_preserved_per_line()
    {
        EndOfLineTrim.Trim("ab \r\ncd \nef ").Should().Be("ab\r\ncd\nef");
    }
}
