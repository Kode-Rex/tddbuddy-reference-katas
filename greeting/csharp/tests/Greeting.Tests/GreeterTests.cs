using FluentAssertions;
using Xunit;

namespace Greeting.Tests;

public class GreeterTests
{
    [Fact]
    public void A_single_name_is_greeted_by_name()
    {
        Greeter.Greet("Bob").Should().Be("Hello, Bob.");
    }

    [Fact]
    public void A_null_name_is_greeted_as_my_friend()
    {
        Greeter.Greet((string?)null).Should().Be("Hello, my friend.");
    }

    [Fact]
    public void An_all_caps_name_is_shouted_back()
    {
        Greeter.Greet("JERRY").Should().Be("HELLO JERRY!");
    }

    [Fact]
    public void Two_names_are_joined_with_and()
    {
        Greeter.Greet(new string?[] { "Jill", "Jane" }).Should().Be("Hello, Jill and Jane");
    }

    [Fact]
    public void Three_or_more_names_are_joined_with_an_Oxford_comma()
    {
        Greeter.Greet(new string?[] { "Amy", "Brian", "Charlotte" }).Should().Be("Hello, Amy, Brian, and Charlotte");
    }

    [Fact]
    public void Mixed_normal_and_shouted_names_split_into_two_greetings()
    {
        Greeter.Greet(new string?[] { "Amy", "BRIAN", "Charlotte" }).Should().Be("Hello, Amy and Charlotte. AND HELLO BRIAN!");
    }
}
