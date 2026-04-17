from markdown_parser import parse

from .document_builder import DocumentBuilder


# --- Empty and Plain Text ---


def test_empty_input_returns_empty_output():
    assert parse("") == ""


def test_plain_text_is_wrapped_in_a_paragraph():
    assert parse("Hello") == "<p>Hello</p>"


def test_multiple_words_in_plain_text_are_preserved():
    assert parse("Hello World") == "<p>Hello World</p>"


# --- Headings ---


def test_single_hash_with_space_produces_h1():
    assert parse("# Heading") == "<h1>Heading</h1>"


def test_double_hash_with_space_produces_h2():
    assert parse("## Sub Heading") == "<h2>Sub Heading</h2>"


def test_six_hashes_with_space_produces_h6():
    assert parse("###### Smallest") == "<h6>Smallest</h6>"


def test_hash_without_space_is_treated_as_a_paragraph():
    assert parse("#No space") == "<p>#No space</p>"


def test_inline_formatting_inside_headings_is_applied():
    assert parse("# **Bold** Heading") == "<h1><strong>Bold</strong> Heading</h1>"


# --- Bold ---


def test_double_asterisks_produce_strong():
    assert parse("**bold**") == "<p><strong>bold</strong></p>"


def test_bold_text_surrounded_by_plain_text():
    assert parse("before **bold** after") == "<p>before <strong>bold</strong> after</p>"


# --- Italic ---


def test_underscores_produce_emphasis():
    assert parse("_italic_") == "<p><em>italic</em></p>"


def test_italic_text_surrounded_by_plain_text():
    assert parse("before _italic_ after") == "<p>before <em>italic</em> after</p>"


# --- Nested Inline Formatting ---


def test_bold_wrapping_italic_produces_nested_tags():
    assert parse("**_bold italic_**") == "<p><strong><em>bold italic</em></strong></p>"


def test_bold_and_italic_in_the_same_line():
    assert parse("**bold** and _italic_") == "<p><strong>bold</strong> and <em>italic</em></p>"


# --- Links ---


def test_link_syntax_produces_anchor_tag():
    assert parse("[click](http://example.com)") == '<p><a href="http://example.com">click</a></p>'


def test_link_surrounded_by_plain_text():
    assert (
        parse("see [here](http://example.com) now")
        == '<p>see <a href="http://example.com">here</a> now</p>'
    )


# --- Unordered Lists ---


def test_single_list_item_produces_ul_with_one_li():
    assert parse("- item one") == "<ul><li>item one</li></ul>"


def test_multiple_consecutive_list_items_grouped_into_one_ul():
    markdown = (
        DocumentBuilder()
        .with_list_item("item one")
        .with_list_item("item two")
        .with_list_item("item three")
        .build()
    )
    assert parse(markdown) == "<ul><li>item one</li><li>item two</li><li>item three</li></ul>"


def test_inline_formatting_inside_list_items_is_applied():
    assert parse("- **bold** item") == "<ul><li><strong>bold</strong> item</li></ul>"


# --- Multiple Paragraphs ---


def test_blank_line_separates_two_paragraphs():
    markdown = (
        DocumentBuilder()
        .with_paragraph("first")
        .with_blank_line()
        .with_paragraph("second")
        .build()
    )
    assert parse(markdown) == "<p>first</p><p>second</p>"


# --- Inline Code ---


def test_backtick_wrapped_text_produces_code_element():
    assert parse("`code`") == "<p><code>code</code></p>"


def test_inline_code_does_not_process_nested_formatting():
    assert parse("`**not bold**`") == "<p><code>**not bold**</code></p>"


# --- Code Blocks ---


def test_fenced_code_block_produces_pre_and_code_elements():
    markdown = DocumentBuilder().with_code_block("let x = 1;").build()
    assert parse(markdown) == "<pre><code>let x = 1;</code></pre>"


def test_code_block_preserves_content_verbatim():
    markdown = DocumentBuilder().with_code_block("**not bold**").build()
    assert parse(markdown) == "<pre><code>**not bold**</code></pre>"


# --- Blockquotes ---


def test_line_starting_with_angle_bracket_produces_blockquote():
    assert parse("> quoted text") == "<blockquote>quoted text</blockquote>"


# --- Escaped Characters ---


def test_escaped_asterisks_render_as_literal_asterisks():
    assert parse("\\*not bold\\*") == "<p>*not bold*</p>"


def test_escaped_underscores_render_as_literal_underscores():
    assert parse("\\_not italic\\_") == "<p>_not italic_</p>"
