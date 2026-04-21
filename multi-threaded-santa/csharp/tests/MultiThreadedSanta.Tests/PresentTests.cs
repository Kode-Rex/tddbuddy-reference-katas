using FluentAssertions;
using Xunit;

namespace MultiThreadedSanta.Tests;

public class PresentTests
{
    [Fact]
    public void A_new_present_starts_in_the_unmade_state()
    {
        var present = new Present(1);

        present.State.Should().Be(PresentState.Unmade);
    }

    [Fact]
    public void Making_a_present_transitions_it_to_the_made_state()
    {
        var present = new Present(1);

        present.Make();

        present.State.Should().Be(PresentState.Made);
    }

    [Fact]
    public void Wrapping_a_made_present_transitions_it_to_the_wrapped_state()
    {
        var present = new Present(1);
        present.Make();

        present.Wrap();

        present.State.Should().Be(PresentState.Wrapped);
    }

    [Fact]
    public void Loading_a_wrapped_present_transitions_it_to_the_loaded_state()
    {
        var present = new Present(1);
        present.Make();
        present.Wrap();

        present.Load();

        present.State.Should().Be(PresentState.Loaded);
    }

    [Fact]
    public void Delivering_a_loaded_present_transitions_it_to_the_delivered_state()
    {
        var present = new Present(1);
        present.Make();
        present.Wrap();
        present.Load();

        present.Deliver();

        present.State.Should().Be(PresentState.Delivered);
    }
}
