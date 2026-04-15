from end_of_line_trim import trim


def test_no_trailing_whitespace_is_returned_unchanged():
    assert trim("abc") == "abc"


def test_trailing_space_is_removed():
    assert trim("abc ") == "abc"


def test_trailing_tab_is_removed():
    assert trim("abc\t") == "abc"


def test_leading_whitespace_is_preserved():
    assert trim(" abc") == " abc"


def test_trailing_whitespace_is_removed_on_each_crlf_line():
    assert trim("ab\r\n cd \r\n") == "ab\r\n cd\r\n"


def test_a_lone_crlf_is_returned_unchanged():
    assert trim("\r\n") == "\r\n"


def test_trailing_whitespace_is_removed_on_each_lf_line():
    assert trim("ab\n cd \n") == "ab\n cd\n"


def test_whitespace_only_line_collapses_but_keeps_its_terminator():
    assert trim("  \n") == "\n"


def test_an_empty_string_returns_an_empty_string():
    assert trim("") == ""


def test_mixed_line_endings_are_preserved_per_line():
    assert trim("ab \r\ncd \nef ") == "ab\r\ncd\nef"
