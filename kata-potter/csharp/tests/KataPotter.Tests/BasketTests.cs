using FluentAssertions;
using Xunit;

namespace KataPotter.Tests;

public class BasketTests
{
    [Fact]
    public void Empty_basket_costs_zero()
    {
        var basket = new BasketBuilder().Build();

        basket.Price().Should().Be(0.00m);
    }

    [Fact]
    public void One_book_costs_the_base_price()
    {
        var basket = new BasketBuilder().WithBook(1, 1).Build();

        basket.Price().Should().Be(8.00m);
    }

    [Fact]
    public void Two_copies_of_the_same_book_get_no_discount()
    {
        var basket = new BasketBuilder().WithBook(1, 2).Build();

        basket.Price().Should().Be(16.00m);
    }

    [Fact]
    public void Two_distinct_books_get_the_five_percent_discount()
    {
        var basket = new BasketBuilder()
            .WithBook(1, 1)
            .WithBook(2, 1)
            .Build();

        basket.Price().Should().Be(15.20m);
    }

    [Fact]
    public void Three_distinct_books_get_the_ten_percent_discount()
    {
        var basket = new BasketBuilder()
            .WithBook(1, 1).WithBook(2, 1).WithBook(3, 1)
            .Build();

        basket.Price().Should().Be(21.60m);
    }

    [Fact]
    public void Four_distinct_books_get_the_twenty_percent_discount()
    {
        var basket = new BasketBuilder()
            .WithBook(1, 1).WithBook(2, 1).WithBook(3, 1).WithBook(4, 1)
            .Build();

        basket.Price().Should().Be(25.60m);
    }

    [Fact]
    public void Five_distinct_books_get_the_twenty_five_percent_discount()
    {
        var basket = new BasketBuilder()
            .WithBook(1, 1).WithBook(2, 1).WithBook(3, 1).WithBook(4, 1).WithBook(5, 1)
            .Build();

        basket.Price().Should().Be(30.00m);
    }

    [Fact]
    public void Duplicates_are_priced_separately_from_the_discounted_set()
    {
        var basket = new BasketBuilder()
            .WithBook(1, 2)
            .WithBook(2, 1)
            .Build();

        // one 2-set (€15.20) + one 1-set (€8.00)
        basket.Price().Should().Be(23.20m);
    }

    [Fact]
    public void Two_copies_of_every_book_makes_two_five_sets()
    {
        var basket = new BasketBuilder()
            .WithBook(1, 2).WithBook(2, 2).WithBook(3, 2).WithBook(4, 2).WithBook(5, 2)
            .Build();

        basket.Price().Should().Be(60.00m);
    }

    [Fact]
    public void Greedy_fails_basket_prefers_two_four_sets_over_a_five_plus_three()
    {
        // two each of books 1,2,3 plus one each of 4,5
        var basket = new BasketBuilder()
            .WithBook(1, 2).WithBook(2, 2).WithBook(3, 2)
            .WithBook(4, 1).WithBook(5, 1)
            .Build();

        // Two 4-sets (€25.60 each) beats a 5-set (€30.00) + 3-set (€21.60).
        basket.Price().Should().Be(51.20m);
    }

    [Fact]
    public void Bigger_greedy_fails_basket()
    {
        // three each of 1,2,3 plus two each of 4,5
        var basket = new BasketBuilder()
            .WithBook(1, 3).WithBook(2, 3).WithBook(3, 3)
            .WithBook(4, 2).WithBook(5, 2)
            .Build();

        // One 5-set (€30.00) + two 4-sets (€25.60 each) = €81.20.
        // Greedy would price this as two 5-sets + one 3-set = €81.60.
        basket.Price().Should().Be(81.20m);
    }

    [Fact]
    public void BasketBuilder_rejects_book_ids_outside_1_to_5()
    {
        var below = () => new BasketBuilder().WithBook(0, 1);
        var above = () => new BasketBuilder().WithBook(6, 1);

        below.Should().Throw<BookOutOfRangeException>()
            .WithMessage("book id must be between 1 and 5*");
        above.Should().Throw<BookOutOfRangeException>()
            .WithMessage("book id must be between 1 and 5*");
    }
}
