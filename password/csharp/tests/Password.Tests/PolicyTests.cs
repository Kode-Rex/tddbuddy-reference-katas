using FluentAssertions;
using Xunit;

namespace Password.Tests;

public class PolicyTests
{
    [Fact]
    public void Policy_with_minLength_8_accepts_an_8_character_password()
    {
        var policy = new PolicyBuilder().MinLength(8).Build();

        policy.Validate("abcd1234").Ok.Should().BeTrue();
    }

    [Fact]
    public void Policy_with_minLength_8_rejects_a_6_character_password()
    {
        var policy = new PolicyBuilder().MinLength(8).Build();

        var result = policy.Validate("abc123");

        result.Ok.Should().BeFalse();
        result.Failures.Should().ContainSingle().Which.Should().Be("minimum length");
    }

    [Fact]
    public void Policy_requiring_a_digit_accepts_a_password_with_a_digit()
    {
        var policy = new PolicyBuilder().RequiresDigit().Build();

        policy.Validate("password1").Ok.Should().BeTrue();
    }

    [Fact]
    public void Policy_requiring_a_digit_rejects_a_password_with_no_digit()
    {
        var policy = new PolicyBuilder().RequiresDigit().Build();

        var result = policy.Validate("password");

        result.Ok.Should().BeFalse();
        result.Failures.Should().ContainSingle().Which.Should().Be("requires digit");
    }

    [Fact]
    public void Policy_requiring_a_symbol_accepts_a_password_with_a_symbol()
    {
        var policy = new PolicyBuilder().RequiresSymbol().Build();

        policy.Validate("password!").Ok.Should().BeTrue();
    }

    [Fact]
    public void Policy_requiring_a_symbol_rejects_a_password_with_no_symbol()
    {
        var policy = new PolicyBuilder().RequiresSymbol().Build();

        var result = policy.Validate("password1");

        result.Ok.Should().BeFalse();
        result.Failures.Should().ContainSingle().Which.Should().Be("requires symbol");
    }

    [Fact]
    public void Policy_requiring_uppercase_rejects_an_all_lowercase_password()
    {
        var policy = new PolicyBuilder().RequiresUpper().Build();

        var result = policy.Validate("password1");

        result.Ok.Should().BeFalse();
        result.Failures.Should().ContainSingle().Which.Should().Be("requires uppercase");
    }

    [Fact]
    public void Policy_requiring_lowercase_rejects_an_all_uppercase_password()
    {
        var policy = new PolicyBuilder().RequiresLower().Build();

        var result = policy.Validate("PASSWORD1");

        result.Ok.Should().BeFalse();
        result.Failures.Should().ContainSingle().Which.Should().Be("requires lowercase");
    }

    [Fact]
    public void Policy_with_multiple_requirements_reports_every_failed_rule()
    {
        var policy = new PolicyBuilder()
            .MinLength(8)
            .RequiresDigit()
            .RequiresSymbol()
            .RequiresUpper()
            .RequiresLower()
            .Build();

        policy.Validate("Abcd123!").Ok.Should().BeTrue();

        var result = policy.Validate("abc");
        result.Ok.Should().BeFalse();
        result.Failures.Should().Equal(
            "minimum length",
            "requires digit",
            "requires symbol",
            "requires uppercase");
    }

    [Fact]
    public void PolicyBuilder_default_is_minLength_8_with_no_character_class_requirements()
    {
        var policy = new PolicyBuilder().Build();

        policy.Validate("abcdefgh").Ok.Should().BeTrue();

        var result = policy.Validate("abcdefg");
        result.Ok.Should().BeFalse();
        result.Failures.Should().ContainSingle().Which.Should().Be("minimum length");
    }
}
