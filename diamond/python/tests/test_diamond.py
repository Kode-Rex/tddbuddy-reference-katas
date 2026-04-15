import pytest

from diamond import print_diamond


def test_a_is_a_single_letter_diamond():
    assert print_diamond("A") == "A"


def test_b_renders_three_rows_with_a_single_inner_space():
    assert print_diamond("B") == " A\nB B\n A"


def test_c_renders_five_rows_and_three_inner_spaces_on_the_widest_row():
    assert print_diamond("C") == "  A\n B B\nC   C\n B B\n  A"


def test_d_renders_seven_rows_with_a_five_space_widest_row():
    expected = "\n".join(
        [
            "   A",
            "  B B",
            " C   C",
            "D     D",
            " C   C",
            "  B B",
            "   A",
        ]
    )
    assert print_diamond("D") == expected


def test_e_renders_nine_rows_with_a_seven_space_widest_row():
    expected = "\n".join(
        [
            "    A",
            "   B B",
            "  C   C",
            " D     D",
            "E       E",
            " D     D",
            "  C   C",
            "   B B",
            "    A",
        ]
    )
    assert print_diamond("E") == expected


def test_z_renders_a_full_fifty_one_row_diamond():
    rows = print_diamond("Z").split("\n")
    assert len(rows) == 51
    assert rows[0] == " " * 25 + "A"
    assert rows[25] == "Z" + " " * 49 + "Z"
    assert rows[50] == " " * 25 + "A"


def test_lowercase_input_is_normalized_to_uppercase():
    assert print_diamond("c") == print_diamond("C")


def test_top_half_mirrors_bottom_half():
    rows = print_diamond("F").split("\n")
    last = len(rows) - 1
    for r in range(last // 2 + 1):
        assert rows[r] == rows[last - r]


def test_no_row_has_trailing_whitespace():
    for row in print_diamond("G").split("\n"):
        assert row == row.rstrip()


def test_non_letter_input_is_rejected():
    with pytest.raises(ValueError, match="letter must be a single A-Z character"):
        print_diamond("1")
