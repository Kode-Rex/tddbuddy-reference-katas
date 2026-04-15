using FluentAssertions;

namespace LibraryManagement.Tests;

public class MemberTests
{
    private static readonly DateOnly Day0 = new(2026, 1, 1);

    [Fact]
    public void New_library_has_no_members()
    {
        var (library, _, _) = new LibraryBuilder().OpenedOn(Day0).Build();

        library.Members.Should().BeEmpty();
    }

    [Fact]
    public void Registering_a_member_adds_them_to_the_library()
    {
        var (library, _, _) = new LibraryBuilder().OpenedOn(Day0).Build();

        var member = library.Register("Ada Lovelace");

        library.Members.Should().ContainSingle().Which.Should().BeSameAs(member);
        member.Name.Should().Be("Ada Lovelace");
    }
}
