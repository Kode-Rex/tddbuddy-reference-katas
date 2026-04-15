using FluentAssertions;
using Xunit;

namespace BankOcr.Tests;

public class BankOcrTests
{
    // --- Digit Parsing ---------------------------------------------------

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    [InlineData(9)]
    public void Parses_the_canonical_glyph_for_each_digit(int value)
    {
        var glyph = new DigitBuilder().ForDigit(value).Build();

        Parser.ParseDigit(glyph).Should().Be(Digit.Of(value));
    }

    [Fact]
    public void A_non_canonical_glyph_parses_as_unknown()
    {
        var garbled = new DigitBuilder().ForDigit(8).WithRow(1, "|X|").Build();

        Parser.ParseDigit(garbled).Should().Be(Digit.Unknown);
    }

    // --- Account Number Parsing -----------------------------------------

    [Fact]
    public void Parses_a_full_9_digit_account_number_from_a_3x27_block()
    {
        var rows = new AccountNumberBuilder().FromString("123456789").BuildRows();

        var account = Parser.ParseAccountNumber(rows);

        account.Number.Should().Be("123456789");
        account.IsLegible.Should().BeTrue();
    }

    [Fact]
    public void An_account_with_one_unreadable_digit_parses_with_an_unknown_in_that_position()
    {
        var garbled = new DigitBuilder().ForDigit(9).WithRow(2, "|X|").Build();
        var rows = new AccountNumberBuilder().FromString("123456789").WithDigitAt(8, garbled).BuildRows();

        var account = Parser.ParseAccountNumber(rows);

        account.Number.Should().Be("12345678?");
        account.IsLegible.Should().BeFalse();
    }

    [Fact]
    public void Rejects_an_OCR_block_with_the_wrong_number_of_rows()
    {
        var twoRows = new[] { new string(' ', 27), new string(' ', 27) };

        Action act = () => Parser.ParseAccountNumber(twoRows);

        act.Should().Throw<InvalidAccountNumberFormatException>();
    }

    [Fact]
    public void Rejects_an_OCR_block_with_the_wrong_row_width()
    {
        var rows = new[] { new string(' ', 26), new string(' ', 27), new string(' ', 27) };

        Action act = () => Parser.ParseAccountNumber(rows);

        act.Should().Throw<InvalidAccountNumberFormatException>();
    }

    // --- Checksum Validation ---------------------------------------------

    [Fact]
    public void A_legible_account_with_a_valid_checksum_reports_as_valid()
    {
        var rows = new AccountNumberBuilder().FromString("345882865").BuildRows();

        Parser.ParseAccountNumber(rows).IsChecksumValid.Should().BeTrue();
    }

    [Fact]
    public void A_legible_account_with_an_invalid_checksum_reports_as_invalid()
    {
        var rows = new AccountNumberBuilder().FromString("345882866").BuildRows();

        Parser.ParseAccountNumber(rows).IsChecksumValid.Should().BeFalse();
    }

    [Fact]
    public void An_account_containing_an_unknown_digit_is_not_considered_for_checksum()
    {
        var garbled = new DigitBuilder().ForDigit(5).WithRow(0, "X_X").Build();
        var rows = new AccountNumberBuilder().FromString("345882865").WithDigitAt(8, garbled).BuildRows();

        Parser.ParseAccountNumber(rows).IsChecksumValid.Should().BeFalse();
    }

    // --- Status Reporting ------------------------------------------------

    [Fact]
    public void Status_for_a_valid_account_is_just_the_number()
    {
        var rows = new AccountNumberBuilder().FromString("345882865").BuildRows();

        Parser.ParseAccountNumber(rows).Status.Should().Be("345882865");
    }

    [Fact]
    public void Status_for_a_bad_checksum_account_appends_ERR()
    {
        var rows = new AccountNumberBuilder().FromString("345882866").BuildRows();

        Parser.ParseAccountNumber(rows).Status.Should().Be("345882866 ERR");
    }

    [Fact]
    public void Status_for_an_illegible_account_appends_ILL()
    {
        var garbled = new DigitBuilder().ForDigit(5).WithRow(0, "X_X").Build();
        var rows = new AccountNumberBuilder().FromString("345882865").WithDigitAt(8, garbled).BuildRows();

        Parser.ParseAccountNumber(rows).Status.Should().Be("34588286? ILL");
    }

    // --- Builders --------------------------------------------------------

    [Fact]
    public void AccountNumberBuilder_fromString_renders_a_3x27_OCR_block_matching_the_canonical_glyphs()
    {
        var rows = new AccountNumberBuilder().FromString("123456789").BuildRows();

        rows.Should().HaveCount(3);
        rows[0].Should().Be("    _  _     _  _  _  _  _ ");
        rows[1].Should().Be("  | _| _||_||_ |_   ||_||_|");
        rows[2].Should().Be("  ||_  _|  | _||_|  ||_| _|");
    }
}
