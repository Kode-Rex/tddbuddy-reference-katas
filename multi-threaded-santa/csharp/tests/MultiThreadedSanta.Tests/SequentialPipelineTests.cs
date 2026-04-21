using FluentAssertions;
using Xunit;

namespace MultiThreadedSanta.Tests;

public class SequentialPipelineTests
{
    [Fact]
    public async Task A_single_present_flows_through_all_four_stages()
    {
        var pipeline = new Pipeline(madeCapacity: 10, wrappedCapacity: 10, loadedCapacity: 10);
        var present = new Present(1);

        await pipeline.ProcessSequentiallyAsync([present]);

        present.State.Should().Be(PresentState.Delivered);
        pipeline.Delivered.Should().ContainSingle().Which.Id.Should().Be(1);
    }

    [Fact]
    public async Task Multiple_presents_all_complete_the_full_pipeline()
    {
        var pipeline = new Pipeline(madeCapacity: 10, wrappedCapacity: 10, loadedCapacity: 10);
        var presents = Enumerable.Range(1, 10).Select(i => new Present(i)).ToList();

        await pipeline.ProcessSequentiallyAsync(presents);

        presents.Should().OnlyContain(p => p.State == PresentState.Delivered);
        pipeline.Delivered.Should().HaveCount(10);
    }

    [Fact]
    public async Task Presents_emerge_from_the_pipeline_in_order()
    {
        var pipeline = new Pipeline(madeCapacity: 10, wrappedCapacity: 10, loadedCapacity: 10);
        var presents = Enumerable.Range(1, 5).Select(i => new Present(i)).ToList();

        await pipeline.ProcessSequentiallyAsync(presents);

        pipeline.Delivered.Select(p => p.Id).Should().Equal(1, 2, 3, 4, 5);
    }
}
