using FluentAssertions;
using Xunit;

namespace Pagination.Tests;

public class PageRequestTests
{
    [Fact]
    public void First_page_of_100_items_with_page_size_10_shows_items_1_through_10()
    {
        var request = new PageRequestBuilder().TotalItems(100).PageSize(10).PageNumber(1).Build();

        request.StartItem.Should().Be(1);
        request.EndItem.Should().Be(10);
        request.HasPrevious.Should().BeFalse();
        request.HasNext.Should().BeTrue();
        request.TotalPages.Should().Be(10);
    }

    [Fact]
    public void Middle_page_of_100_items_with_page_size_10_shows_items_41_through_50()
    {
        var request = new PageRequestBuilder().TotalItems(100).PageSize(10).PageNumber(5).Build();

        request.StartItem.Should().Be(41);
        request.EndItem.Should().Be(50);
        request.HasPrevious.Should().BeTrue();
        request.HasNext.Should().BeTrue();
    }

    [Fact]
    public void Last_page_of_100_items_with_page_size_10_shows_items_91_through_100()
    {
        var request = new PageRequestBuilder().TotalItems(100).PageSize(10).PageNumber(10).Build();

        request.StartItem.Should().Be(91);
        request.EndItem.Should().Be(100);
        request.HasPrevious.Should().BeTrue();
        request.HasNext.Should().BeFalse();
    }

    [Fact]
    public void Last_page_with_a_partial_window_shows_only_the_remaining_items()
    {
        var request = new PageRequestBuilder().TotalItems(95).PageSize(10).PageNumber(10).Build();

        request.StartItem.Should().Be(91);
        request.EndItem.Should().Be(95);
        request.TotalPages.Should().Be(10);
    }

    [Fact]
    public void Single_item_on_one_page_reports_itself_as_start_and_end()
    {
        var request = new PageRequestBuilder().TotalItems(1).PageSize(10).PageNumber(1).Build();

        request.StartItem.Should().Be(1);
        request.EndItem.Should().Be(1);
        request.HasPrevious.Should().BeFalse();
        request.HasNext.Should().BeFalse();
        request.TotalPages.Should().Be(1);
    }

    [Fact]
    public void Zero_items_reports_no_pages_and_no_items()
    {
        var request = new PageRequestBuilder().TotalItems(0).PageSize(10).PageNumber(1).Build();

        request.TotalPages.Should().Be(0);
        request.StartItem.Should().Be(0);
        request.EndItem.Should().Be(0);
        request.HasPrevious.Should().BeFalse();
        request.HasNext.Should().BeFalse();
    }

    [Fact]
    public void Page_number_below_1_is_clamped_to_1()
    {
        var request = new PageRequestBuilder().TotalItems(100).PageSize(10).PageNumber(0).Build();

        request.PageNumber.Should().Be(1);
        request.StartItem.Should().Be(1);
        request.EndItem.Should().Be(10);
    }

    [Fact]
    public void Page_number_above_totalPages_is_clamped_to_the_last_page()
    {
        var request = new PageRequestBuilder().TotalItems(100).PageSize(10).PageNumber(99).Build();

        request.PageNumber.Should().Be(10);
        request.StartItem.Should().Be(91);
        request.EndItem.Should().Be(100);
    }

    [Fact]
    public void Page_size_of_1_gives_every_item_its_own_page()
    {
        var request = new PageRequestBuilder().TotalItems(5).PageSize(1).PageNumber(3).Build();

        request.StartItem.Should().Be(3);
        request.EndItem.Should().Be(3);
        request.TotalPages.Should().Be(5);
        request.HasPrevious.Should().BeTrue();
        request.HasNext.Should().BeTrue();
    }

    [Fact]
    public void Negative_totalItems_is_rejected_at_construction()
    {
        var act = () => new PageRequestBuilder().TotalItems(-1).Build();

        act.Should().Throw<ArgumentException>().WithMessage("totalItems must be >= 0*");
    }

    [Fact]
    public void Page_size_below_1_is_rejected_at_construction()
    {
        var act = () => new PageRequestBuilder().PageSize(0).Build();

        act.Should().Throw<ArgumentException>().WithMessage("pageSize must be >= 1*");
    }

    [Fact]
    public void PageWindow_centers_on_the_current_page_when_there_is_room()
    {
        var first = new PageRequestBuilder().TotalItems(100).PageSize(10).PageNumber(1).Build();
        var middle = new PageRequestBuilder().TotalItems(100).PageSize(10).PageNumber(5).Build();
        var last = new PageRequestBuilder().TotalItems(100).PageSize(10).PageNumber(10).Build();

        first.PageWindow(5).Should().Equal(1, 2, 3, 4, 5);
        middle.PageWindow(5).Should().Equal(3, 4, 5, 6, 7);
        last.PageWindow(5).Should().Equal(6, 7, 8, 9, 10);
    }

    [Fact]
    public void PageWindow_is_clipped_when_totalPages_is_smaller_than_the_window()
    {
        var request = new PageRequestBuilder().TotalItems(40).PageSize(10).PageNumber(3).Build();

        request.PageWindow(5).Should().Equal(1, 2, 3, 4);
    }

    [Fact]
    public void PageWindow_on_an_empty_dataset_is_empty()
    {
        var request = new PageRequestBuilder().TotalItems(0).PageSize(10).PageNumber(1).Build();

        request.PageWindow(5).Should().BeEmpty();
    }
}
