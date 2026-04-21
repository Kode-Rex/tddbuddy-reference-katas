using FluentAssertions;
using Xunit;

namespace MultiThreadedSanta.Tests;

public class BoundedQueueTests
{
    [Fact]
    public async Task Enqueuing_an_item_makes_it_available_for_dequeue()
    {
        var queue = new BoundedQueue<int>(10);

        await queue.EnqueueAsync(42);
        var (success, item) = await queue.DequeueAsync();

        success.Should().BeTrue();
        item.Should().Be(42);
    }

    [Fact]
    public async Task Dequeuing_returns_items_in_FIFO_order()
    {
        var queue = new BoundedQueue<int>(10);

        await queue.EnqueueAsync(1);
        await queue.EnqueueAsync(2);
        await queue.EnqueueAsync(3);

        var (_, first) = await queue.DequeueAsync();
        var (_, second) = await queue.DequeueAsync();
        var (_, third) = await queue.DequeueAsync();

        first.Should().Be(1);
        second.Should().Be(2);
        third.Should().Be(3);
    }

    [Fact]
    public async Task A_queue_reports_its_current_count()
    {
        var queue = new BoundedQueue<int>(10);

        queue.Count.Should().Be(0);

        await queue.EnqueueAsync(1);
        await queue.EnqueueAsync(2);

        queue.Count.Should().Be(2);

        await queue.DequeueAsync();

        queue.Count.Should().Be(1);
    }

    [Fact]
    public void A_full_queue_rejects_further_enqueues_until_space_is_available()
    {
        var queue = new BoundedQueue<int>(2);

        queue.TryEnqueue(1).Should().BeTrue();
        queue.TryEnqueue(2).Should().BeTrue();
        queue.TryEnqueue(3).Should().BeFalse("queue is at capacity");

        queue.Count.Should().Be(2);
    }
}
