using FluentAssertions;
using Xunit;

namespace LinkedListKata.Tests;

public class LinkedListTests
{
    [Fact]
    public void A_new_list_is_empty()
    {
        var list = new LinkedList<int>();

        list.Size().Should().Be(0);
        list.ToArray().Should().BeEmpty();
    }

    [Fact]
    public void Appending_to_an_empty_list_yields_a_single_element()
    {
        var list = new LinkedList<int>();

        list.Append(1);

        list.Size().Should().Be(1);
        list.ToArray().Should().Equal(1);
    }

    [Fact]
    public void Appending_preserves_insertion_order()
    {
        var list = new LinkedList<int>();

        list.Append(1);
        list.Append(2);
        list.Append(3);

        list.ToArray().Should().Equal(1, 2, 3);
    }

    [Fact]
    public void Prepending_to_an_empty_list_yields_a_single_element()
    {
        var list = new LinkedList<int>();

        list.Prepend(1);

        list.ToArray().Should().Equal(1);
    }

    [Fact]
    public void Prepending_puts_the_value_at_the_front()
    {
        var list = new LinkedList<int>();
        list.Append(1);
        list.Append(2);

        list.Prepend(0);

        list.ToArray().Should().Equal(0, 1, 2);
    }

    [Fact]
    public void Get_returns_the_value_at_the_given_index()
    {
        var list = ListOf(0, 1, 2);

        list.Get(0).Should().Be(0);
        list.Get(2).Should().Be(2);
    }

    [Fact]
    public void Get_on_an_out_of_range_index_raises()
    {
        var list = ListOf(0, 1, 2);

        list.Invoking(l => l.Get(5))
            .Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("index out of range: 5*");
    }

    [Fact]
    public void Get_on_a_negative_index_raises()
    {
        var list = ListOf(0, 1, 2);

        list.Invoking(l => l.Get(-1))
            .Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("index out of range: -1*");
    }

    [Fact]
    public void Remove_returns_the_value_and_shifts_subsequent_elements()
    {
        var list = ListOf(0, 1, 2);

        var removed = list.Remove(1);

        removed.Should().Be(1);
        list.ToArray().Should().Equal(0, 2);
    }

    [Fact]
    public void Remove_the_head_returns_the_first_value_and_leaves_the_tail()
    {
        var list = ListOf(0, 2);

        var removed = list.Remove(0);

        removed.Should().Be(0);
        list.ToArray().Should().Equal(2);
    }

    [Fact]
    public void Remove_the_only_element_leaves_an_empty_list()
    {
        var list = ListOf(2);

        var removed = list.Remove(0);

        removed.Should().Be(2);
        list.Size().Should().Be(0);
        list.ToArray().Should().BeEmpty();
    }

    [Fact]
    public void Remove_on_an_empty_list_raises()
    {
        var list = new LinkedList<int>();

        list.Invoking(l => l.Remove(0))
            .Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("index out of range: 0*");
    }

    [Fact]
    public void Contains_finds_an_existing_value()
    {
        var list = ListOf(2);

        list.Contains(2).Should().BeTrue();
        list.Contains(99).Should().BeFalse();
    }

    [Fact]
    public void IndexOf_returns_the_first_occurrence()
    {
        var list = ListOf(2);

        list.IndexOf(2).Should().Be(0);
        list.IndexOf(99).Should().Be(-1);
    }

    [Fact]
    public void InsertAt_the_head_shifts_existing_elements()
    {
        var list = ListOf(2);

        list.InsertAt(0, 5);

        list.ToArray().Should().Equal(5, 2);
    }

    [Fact]
    public void InsertAt_the_middle_shifts_subsequent_elements()
    {
        var list = ListOf(5, 2);

        list.InsertAt(1, 7);

        list.ToArray().Should().Equal(5, 7, 2);
    }

    [Fact]
    public void InsertAt_size_is_equivalent_to_append()
    {
        var list = ListOf(5, 7, 2);

        list.InsertAt(3, 9);

        list.ToArray().Should().Equal(5, 7, 2, 9);
    }

    [Fact]
    public void InsertAt_an_out_of_range_index_raises()
    {
        var list = ListOf(5, 7, 2);

        list.Invoking(l => l.InsertAt(10, 9))
            .Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("index out of range: 10*");

        list.Invoking(l => l.InsertAt(-1, 9))
            .Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("index out of range: -1*");
    }

    private static LinkedList<int> ListOf(params int[] values)
    {
        var list = new LinkedList<int>();
        foreach (var value in values) list.Append(value);
        return list;
    }
}
