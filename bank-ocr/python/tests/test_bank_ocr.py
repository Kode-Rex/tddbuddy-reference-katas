import pytest

from bank_ocr import (
    Digit,
    InvalidAccountNumberFormatException,
    parse_account_number,
    parse_digit,
)

from .account_number_builder import AccountNumberBuilder
from .digit_builder import DigitBuilder


# --- Digit Parsing -------------------------------------------------------


@pytest.mark.parametrize("value", list(range(10)))
def test_parses_the_canonical_glyph_for_each_digit(value: int) -> None:
    glyph = DigitBuilder().for_digit(value).build()
    assert parse_digit(glyph) == Digit.of(value)


def test_a_non_canonical_glyph_parses_as_unknown() -> None:
    garbled = DigitBuilder().for_digit(8).with_row(1, "|X|").build()
    assert parse_digit(garbled) == Digit.unknown()


# --- Account Number Parsing ---------------------------------------------


def test_parses_a_full_9_digit_account_number_from_a_3x27_block() -> None:
    rows = AccountNumberBuilder().from_string("123456789").build_rows()
    account = parse_account_number(rows)
    assert account.number == "123456789"
    assert account.is_legible is True


def test_an_account_with_one_unreadable_digit_parses_with_an_unknown_in_that_position() -> None:
    garbled = DigitBuilder().for_digit(9).with_row(2, "|X|").build()
    rows = (
        AccountNumberBuilder()
        .from_string("123456789")
        .with_digit_at(8, garbled)
        .build_rows()
    )
    account = parse_account_number(rows)
    assert account.number == "12345678?"
    assert account.is_legible is False


def test_rejects_an_ocr_block_with_the_wrong_number_of_rows() -> None:
    two_rows = [" " * 27, " " * 27]
    with pytest.raises(InvalidAccountNumberFormatException):
        parse_account_number(two_rows)


def test_rejects_an_ocr_block_with_the_wrong_row_width() -> None:
    rows = [" " * 26, " " * 27, " " * 27]
    with pytest.raises(InvalidAccountNumberFormatException):
        parse_account_number(rows)


# --- Checksum Validation -------------------------------------------------


def test_a_legible_account_with_a_valid_checksum_reports_as_valid() -> None:
    rows = AccountNumberBuilder().from_string("345882865").build_rows()
    assert parse_account_number(rows).is_checksum_valid is True


def test_a_legible_account_with_an_invalid_checksum_reports_as_invalid() -> None:
    rows = AccountNumberBuilder().from_string("345882866").build_rows()
    assert parse_account_number(rows).is_checksum_valid is False


def test_an_account_containing_an_unknown_digit_is_not_considered_for_checksum() -> None:
    garbled = DigitBuilder().for_digit(5).with_row(0, "X_X").build()
    rows = (
        AccountNumberBuilder()
        .from_string("345882865")
        .with_digit_at(8, garbled)
        .build_rows()
    )
    assert parse_account_number(rows).is_checksum_valid is False


# --- Status Reporting ----------------------------------------------------


def test_status_for_a_valid_account_is_just_the_number() -> None:
    rows = AccountNumberBuilder().from_string("345882865").build_rows()
    assert parse_account_number(rows).status == "345882865"


def test_status_for_a_bad_checksum_account_appends_err() -> None:
    rows = AccountNumberBuilder().from_string("345882866").build_rows()
    assert parse_account_number(rows).status == "345882866 ERR"


def test_status_for_an_illegible_account_appends_ill() -> None:
    garbled = DigitBuilder().for_digit(5).with_row(0, "X_X").build()
    rows = (
        AccountNumberBuilder()
        .from_string("345882865")
        .with_digit_at(8, garbled)
        .build_rows()
    )
    assert parse_account_number(rows).status == "34588286? ILL"


# --- Builders ------------------------------------------------------------


def test_account_number_builder_from_string_renders_a_3x27_ocr_block_matching_the_canonical_glyphs() -> None:
    rows = AccountNumberBuilder().from_string("123456789").build_rows()
    assert len(rows) == 3
    assert rows[0] == "    _  _     _  _  _  _  _ "
    assert rows[1] == "  | _| _||_||_ |_   ||_||_|"
    assert rows[2] == "  ||_  _|  | _||_|  ||_| _|"
