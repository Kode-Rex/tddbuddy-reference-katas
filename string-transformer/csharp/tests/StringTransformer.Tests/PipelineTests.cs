using FluentAssertions;
using Xunit;

namespace StringTransformer.Tests;

public class PipelineTests
{
    [Fact]
    public void Empty_pipeline_returns_the_input_unchanged()
    {
        var pipeline = new PipelineBuilder().Build();

        pipeline.Run("hello world").Should().Be("hello world");
    }

    [Fact]
    public void Capitalise_capitalises_each_word()
    {
        var pipeline = new PipelineBuilder().Capitalise().Build();

        pipeline.Run("hello world").Should().Be("Hello World");
    }

    [Fact]
    public void Reverse_reverses_the_whole_string()
    {
        var pipeline = new PipelineBuilder().Reverse().Build();

        pipeline.Run("hello world").Should().Be("dlrow olleh");
    }

    [Fact]
    public void RemoveWhitespace_drops_every_space()
    {
        var pipeline = new PipelineBuilder().RemoveWhitespace().Build();

        pipeline.Run("hello world").Should().Be("helloworld");
    }

    [Fact]
    public void SnakeCase_lowercases_and_joins_with_underscores()
    {
        var pipeline = new PipelineBuilder().SnakeCase().Build();

        pipeline.Run("hello world").Should().Be("hello_world");
    }

    [Fact]
    public void SnakeCase_collapses_hyphens_and_whitespace_into_single_underscores()
    {
        var pipeline = new PipelineBuilder().SnakeCase().Build();

        pipeline.Run("hello-world test").Should().Be("hello_world_test");
    }

    [Fact]
    public void CamelCase_lowercases_the_first_word_and_title_cases_the_rest()
    {
        var pipeline = new PipelineBuilder().CamelCase().Build();

        pipeline.Run("Hello World").Should().Be("helloWorld");
    }

    [Fact]
    public void CamelCase_normalises_all_uppercase_input()
    {
        var pipeline = new PipelineBuilder().CamelCase().Build();

        pipeline.Run("HELLO WORLD").Should().Be("helloWorld");
    }

    [Fact]
    public void Truncate_shortens_long_input_and_appends_the_marker()
    {
        var pipeline = new PipelineBuilder().Truncate(5).Build();

        pipeline.Run("hello world").Should().Be("hello...");
    }

    [Fact]
    public void Truncate_leaves_short_input_untouched()
    {
        var pipeline = new PipelineBuilder().Truncate(50).Build();

        pipeline.Run("hello world").Should().Be("hello world");
    }

    [Fact]
    public void Repeat_produces_n_space_joined_copies()
    {
        var pipeline = new PipelineBuilder().Repeat(3).Build();

        pipeline.Run("ha").Should().Be("ha ha ha");
    }

    [Fact]
    public void Replace_swaps_every_occurrence_of_the_target()
    {
        var pipeline = new PipelineBuilder().Replace("world", "there").Build();

        pipeline.Run("hello world").Should().Be("hello there");
    }

    [Fact]
    public void Chaining_applies_transformations_in_order()
    {
        var pipeline = new PipelineBuilder().Capitalise().Reverse().Build();

        pipeline.Run("hello world").Should().Be("dlroW olleH");
    }

    [Fact]
    public void Chaining_snakeCase_then_capitalise_uppercases_letters_after_underscores()
    {
        var pipeline = new PipelineBuilder().SnakeCase().Capitalise().Build();

        pipeline.Run("hello world").Should().Be("Hello_World");
    }

    [Fact]
    public void Empty_input_survives_capitalise()
    {
        var pipeline = new PipelineBuilder().Capitalise().Build();

        pipeline.Run("").Should().Be("");
    }
}
