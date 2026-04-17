using FluentAssertions;
using Xunit;

namespace MarkdownParser.Tests;

public class MarkdownParserTests
{
    // --- Empty and Plain Text ---

    [Fact]
    public void Empty_input_returns_empty_output()
    {
        MarkdownParser.Parse("").Should().BeEmpty();
    }

    [Fact]
    public void Plain_text_is_wrapped_in_a_paragraph()
    {
        MarkdownParser.Parse("Hello").Should().Be("<p>Hello</p>");
    }

    [Fact]
    public void Multiple_words_in_plain_text_are_preserved()
    {
        MarkdownParser.Parse("Hello World").Should().Be("<p>Hello World</p>");
    }

    // --- Headings ---

    [Fact]
    public void Single_hash_with_space_produces_h1()
    {
        MarkdownParser.Parse("# Heading").Should().Be("<h1>Heading</h1>");
    }

    [Fact]
    public void Double_hash_with_space_produces_h2()
    {
        MarkdownParser.Parse("## Sub Heading").Should().Be("<h2>Sub Heading</h2>");
    }

    [Fact]
    public void Six_hashes_with_space_produces_h6()
    {
        MarkdownParser.Parse("###### Smallest").Should().Be("<h6>Smallest</h6>");
    }

    [Fact]
    public void Hash_without_space_is_treated_as_a_paragraph()
    {
        MarkdownParser.Parse("#No space").Should().Be("<p>#No space</p>");
    }

    [Fact]
    public void Inline_formatting_inside_headings_is_applied()
    {
        MarkdownParser.Parse("# **Bold** Heading")
            .Should().Be("<h1><strong>Bold</strong> Heading</h1>");
    }

    // --- Bold ---

    [Fact]
    public void Double_asterisks_produce_strong()
    {
        MarkdownParser.Parse("**bold**").Should().Be("<p><strong>bold</strong></p>");
    }

    [Fact]
    public void Bold_text_surrounded_by_plain_text()
    {
        MarkdownParser.Parse("before **bold** after")
            .Should().Be("<p>before <strong>bold</strong> after</p>");
    }

    // --- Italic ---

    [Fact]
    public void Underscores_produce_emphasis()
    {
        MarkdownParser.Parse("_italic_").Should().Be("<p><em>italic</em></p>");
    }

    [Fact]
    public void Italic_text_surrounded_by_plain_text()
    {
        MarkdownParser.Parse("before _italic_ after")
            .Should().Be("<p>before <em>italic</em> after</p>");
    }

    // --- Nested Inline Formatting ---

    [Fact]
    public void Bold_wrapping_italic_produces_nested_tags()
    {
        MarkdownParser.Parse("**_bold italic_**")
            .Should().Be("<p><strong><em>bold italic</em></strong></p>");
    }

    [Fact]
    public void Bold_and_italic_in_the_same_line()
    {
        MarkdownParser.Parse("**bold** and _italic_")
            .Should().Be("<p><strong>bold</strong> and <em>italic</em></p>");
    }

    // --- Links ---

    [Fact]
    public void Link_syntax_produces_anchor_tag()
    {
        MarkdownParser.Parse("[click](http://example.com)")
            .Should().Be("<p><a href=\"http://example.com\">click</a></p>");
    }

    [Fact]
    public void Link_surrounded_by_plain_text()
    {
        MarkdownParser.Parse("see [here](http://example.com) now")
            .Should().Be("<p>see <a href=\"http://example.com\">here</a> now</p>");
    }

    // --- Unordered Lists ---

    [Fact]
    public void Single_list_item_produces_ul_with_one_li()
    {
        MarkdownParser.Parse("- item one")
            .Should().Be("<ul><li>item one</li></ul>");
    }

    [Fact]
    public void Multiple_consecutive_list_items_grouped_into_one_ul()
    {
        var markdown = new DocumentBuilder()
            .WithListItem("item one")
            .WithListItem("item two")
            .WithListItem("item three")
            .Build();

        MarkdownParser.Parse(markdown)
            .Should().Be("<ul><li>item one</li><li>item two</li><li>item three</li></ul>");
    }

    [Fact]
    public void Inline_formatting_inside_list_items_is_applied()
    {
        MarkdownParser.Parse("- **bold** item")
            .Should().Be("<ul><li><strong>bold</strong> item</li></ul>");
    }

    // --- Multiple Paragraphs ---

    [Fact]
    public void Blank_line_separates_two_paragraphs()
    {
        var markdown = new DocumentBuilder()
            .WithParagraph("first")
            .WithBlankLine()
            .WithParagraph("second")
            .Build();

        MarkdownParser.Parse(markdown)
            .Should().Be("<p>first</p><p>second</p>");
    }

    // --- Inline Code ---

    [Fact]
    public void Backtick_wrapped_text_produces_code_element()
    {
        MarkdownParser.Parse("`code`").Should().Be("<p><code>code</code></p>");
    }

    [Fact]
    public void Inline_code_does_not_process_nested_formatting()
    {
        MarkdownParser.Parse("`**not bold**`")
            .Should().Be("<p><code>**not bold**</code></p>");
    }

    // --- Code Blocks ---

    [Fact]
    public void Fenced_code_block_produces_pre_and_code_elements()
    {
        var markdown = new DocumentBuilder()
            .WithCodeBlock("let x = 1;")
            .Build();

        MarkdownParser.Parse(markdown)
            .Should().Be("<pre><code>let x = 1;</code></pre>");
    }

    [Fact]
    public void Code_block_preserves_content_verbatim()
    {
        var markdown = new DocumentBuilder()
            .WithCodeBlock("**not bold**")
            .Build();

        MarkdownParser.Parse(markdown)
            .Should().Be("<pre><code>**not bold**</code></pre>");
    }

    // --- Blockquotes ---

    [Fact]
    public void Line_starting_with_angle_bracket_produces_blockquote()
    {
        MarkdownParser.Parse("> quoted text")
            .Should().Be("<blockquote>quoted text</blockquote>");
    }

    // --- Escaped Characters ---

    [Fact]
    public void Escaped_asterisks_render_as_literal_asterisks()
    {
        MarkdownParser.Parse("\\*not bold\\*")
            .Should().Be("<p>*not bold*</p>");
    }

    [Fact]
    public void Escaped_underscores_render_as_literal_underscores()
    {
        MarkdownParser.Parse("\\_not italic\\_")
            .Should().Be("<p>_not italic_</p>");
    }
}
