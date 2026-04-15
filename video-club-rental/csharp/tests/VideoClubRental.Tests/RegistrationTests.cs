using FluentAssertions;
using Xunit;

namespace VideoClubRental.Tests;

public class RegistrationTests
{
    [Fact]
    public void User_aged_eighteen_registers_successfully()
    {
        var (club, _, _) = new VideoClubBuilder().Build();

        var user = club.Register("Eighteen", "eighteen@example.com", new Age(18));

        user.Age.IsAdult.Should().BeTrue();
        club.Users.Should().ContainSingle().Which.Should().BeSameAs(user);
    }

    [Fact]
    public void User_aged_seventeen_is_rejected_as_too_young()
    {
        var (club, _, _) = new VideoClubBuilder().Build();

        var act = () => club.Register("Seventeen", "seventeen@example.com", new Age(17));

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Registration_dispatches_a_welcome_email()
    {
        var (club, notifier, _) = new VideoClubBuilder().Build();

        var user = club.Register("Alex", "alex@example.com", new Age(30));

        notifier.NotificationsFor(user).Should().ContainSingle()
            .Which.Message.Should().Contain("Welcome");
    }

    [Fact]
    public void Admin_creates_another_user_successfully()
    {
        var admin = new UserBuilder().Named("Boss").WithEmail("boss@example.com").AsAdmin().Build();
        var (club, _, _) = new VideoClubBuilder().WithUser(admin).Build();

        var created = club.CreateUser(admin, "New Hire", "new@example.com", new Age(22));

        club.Users.Should().Contain(created);
    }

    [Fact]
    public void Non_admin_attempting_to_create_a_user_is_rejected()
    {
        var regular = new UserBuilder().Build();
        var (club, _, _) = new VideoClubBuilder().WithUser(regular).Build();

        var act = () => club.CreateUser(regular, "New Hire", "new@example.com", new Age(22));

        act.Should().Throw<InvalidOperationException>();
    }
}
