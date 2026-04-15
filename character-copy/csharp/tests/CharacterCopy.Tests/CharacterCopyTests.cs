using FluentAssertions;
using Xunit;

namespace CharacterCopy.Tests;

public class CharacterCopyTests
{
    [Fact]
    public void Immediate_newline_copies_nothing()
    {
        var source = new StringSource("");
        var destination = new RecordingDestination();

        CharacterCopy.Copy(source, destination);

        destination.Written.Should().BeEmpty();
    }

    [Fact]
    public void Single_character_then_newline_copies_that_character()
    {
        var source = new StringSource("a");
        var destination = new RecordingDestination();

        CharacterCopy.Copy(source, destination);

        destination.Written.Should().Be("a");
    }

    [Fact]
    public void Multiple_characters_then_newline_copies_all_of_them_in_order()
    {
        var source = new StringSource("abc");
        var destination = new RecordingDestination();

        CharacterCopy.Copy(source, destination);

        destination.Written.Should().Be("abc");
    }

    [Fact]
    public void Copy_preserves_whitespace_before_the_terminator()
    {
        var source = new StringSource("a b");
        var destination = new RecordingDestination();

        CharacterCopy.Copy(source, destination);

        destination.Written.Should().Be("a b");
    }

    [Fact]
    public void Copy_does_not_read_past_the_terminator()
    {
        var source = new StringSource("abc");
        var destination = new RecordingDestination();

        CharacterCopy.Copy(source, destination);

        source.ReadCount.Should().Be(4);
    }

    [Fact]
    public void Copy_writes_exactly_as_many_characters_as_were_read_before_the_terminator()
    {
        var source = new StringSource("quip");
        var destination = new RecordingDestination();

        CharacterCopy.Copy(source, destination);

        destination.Written.Should().HaveLength(4);
    }
}
