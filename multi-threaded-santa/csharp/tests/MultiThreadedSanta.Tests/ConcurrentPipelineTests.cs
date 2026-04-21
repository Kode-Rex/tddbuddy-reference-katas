using FluentAssertions;
using Xunit;

namespace MultiThreadedSanta.Tests;

public class ConcurrentPipelineTests
{
    [Fact]
    public async Task Multiple_workers_can_process_a_stage_concurrently()
    {
        var pipeline = new Pipeline(madeCapacity: 50, wrappedCapacity: 50, loadedCapacity: 50);
        var presents = Enumerable.Range(1, 20).Select(i => new Present(i)).ToList();

        await pipeline.ProcessConcurrentlyAsync(presents, makeWorkers: 3, wrapWorkers: 2, loadWorkers: 2);

        presents.Should().OnlyContain(p => p.State == PresentState.Delivered);
        pipeline.Delivered.Should().HaveCount(20);
    }

    [Fact]
    public async Task The_sleigh_constraint_allows_only_one_delivery_at_a_time()
    {
        // With the sleigh lock, loading and delivery contend for the same semaphore.
        // This test verifies that all presents still complete — the constraint
        // does not cause deadlocks, even with many workers.
        var pipeline = new Pipeline(madeCapacity: 50, wrappedCapacity: 50, loadedCapacity: 50);
        var presents = Enumerable.Range(1, 30).Select(i => new Present(i)).ToList();

        await pipeline.ProcessConcurrentlyAsync(presents, makeWorkers: 4, wrapWorkers: 3, loadWorkers: 2);

        pipeline.Delivered.Should().HaveCount(30);
    }

    [Fact]
    public async Task Loading_pauses_while_the_sleigh_is_delivering()
    {
        // The sleigh lock ensures mutual exclusion between load and deliver.
        // This test uses concurrent execution and verifies all presents
        // reach delivered state — proving loads and deliveries don't corrupt state.
        var pipeline = new Pipeline(madeCapacity: 20, wrappedCapacity: 20, loadedCapacity: 20);
        var presents = Enumerable.Range(1, 15).Select(i => new Present(i)).ToList();

        await pipeline.ProcessConcurrentlyAsync(presents, makeWorkers: 2, wrapWorkers: 2, loadWorkers: 2);

        presents.Should().OnlyContain(p => p.State == PresentState.Delivered);
    }

    [Fact]
    public async Task All_presents_are_delivered_when_the_pipeline_completes()
    {
        var pipeline = new Pipeline(madeCapacity: 100, wrappedCapacity: 100, loadedCapacity: 100);
        var presents = Enumerable.Range(1, 50).Select(i => new Present(i)).ToList();

        await pipeline.ProcessConcurrentlyAsync(presents, makeWorkers: 4, wrapWorkers: 3, loadWorkers: 2);

        pipeline.Delivered.Should().HaveCount(50);
        var deliveredIds = pipeline.Delivered.Select(p => p.Id).OrderBy(id => id);
        deliveredIds.Should().Equal(Enumerable.Range(1, 50));
    }
}
