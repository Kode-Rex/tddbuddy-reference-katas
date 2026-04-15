using FluentAssertions;
using Xunit;

namespace IpValidator.Tests;

public class IpValidatorTests
{
    [Fact]
    public void Address_1_1_1_1_is_valid()
    {
        IpValidator.IsValid("1.1.1.1").Should().BeTrue();
    }

    [Fact]
    public void Address_192_168_1_1_is_valid()
    {
        IpValidator.IsValid("192.168.1.1").Should().BeTrue();
    }

    [Fact]
    public void Address_10_0_0_1_is_valid()
    {
        IpValidator.IsValid("10.0.0.1").Should().BeTrue();
    }

    [Fact]
    public void Address_127_0_0_1_is_valid()
    {
        IpValidator.IsValid("127.0.0.1").Should().BeTrue();
    }

    [Fact]
    public void Address_0_0_0_0_is_invalid_because_last_octet_is_zero()
    {
        IpValidator.IsValid("0.0.0.0").Should().BeFalse();
    }

    [Fact]
    public void Address_255_255_255_255_is_invalid_because_last_octet_is_broadcast()
    {
        IpValidator.IsValid("255.255.255.255").Should().BeFalse();
    }

    [Fact]
    public void Address_192_168_1_0_is_invalid_because_last_octet_is_zero()
    {
        IpValidator.IsValid("192.168.1.0").Should().BeFalse();
    }

    [Fact]
    public void Address_192_168_1_255_is_invalid_because_last_octet_is_broadcast()
    {
        IpValidator.IsValid("192.168.1.255").Should().BeFalse();
    }

    [Fact]
    public void Address_10_0_1_is_invalid_because_it_has_only_three_octets()
    {
        IpValidator.IsValid("10.0.1").Should().BeFalse();
    }

    [Fact]
    public void Address_1_2_3_4_5_is_invalid_because_it_has_five_octets()
    {
        IpValidator.IsValid("1.2.3.4.5").Should().BeFalse();
    }

    [Fact]
    public void Address_192_168_01_1_is_invalid_because_an_octet_has_a_leading_zero()
    {
        IpValidator.IsValid("192.168.01.1").Should().BeFalse();
    }

    [Fact]
    public void Address_192_168_1_00_is_invalid_because_an_octet_has_leading_zeros()
    {
        IpValidator.IsValid("192.168.1.00").Should().BeFalse();
    }

    [Fact]
    public void Address_256_1_1_1_is_invalid_because_first_octet_exceeds_255()
    {
        IpValidator.IsValid("256.1.1.1").Should().BeFalse();
    }

    [Fact]
    public void Address_1_1_1_999_is_invalid_because_last_octet_exceeds_255()
    {
        IpValidator.IsValid("1.1.1.999").Should().BeFalse();
    }

    [Fact]
    public void Address_with_negative_octet_is_invalid()
    {
        IpValidator.IsValid("1.1.1.-1").Should().BeFalse();
    }

    [Fact]
    public void Address_with_non_digit_character_is_invalid()
    {
        IpValidator.IsValid("1.1.1.a").Should().BeFalse();
    }

    [Fact]
    public void Address_with_empty_octet_is_invalid()
    {
        IpValidator.IsValid("1.1..1").Should().BeFalse();
    }

    [Fact]
    public void Empty_string_is_invalid()
    {
        IpValidator.IsValid("").Should().BeFalse();
    }
}
