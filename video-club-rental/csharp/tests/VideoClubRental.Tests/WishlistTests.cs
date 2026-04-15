using FluentAssertions;
using Xunit;

namespace VideoClubRental.Tests;

public class WishlistTests
{
    [Fact]
    public void User_can_add_a_title_to_their_wishlist()
    {
        var user = new UserBuilder().Build();
        var (club, _, _) = new VideoClubBuilder().WithUser(user).Build();

        club.AddToWishlist(user, "Rushmore");

        user.Wishlist.Should().Contain("Rushmore");
    }

    [Fact]
    public void Wishlist_matching_is_case_insensitive()
    {
        var user = new UserBuilder().Build();
        var donor = new UserBuilder().Named("Donor").WithEmail("d@example.com").Build();
        var (club, notifier, _) = new VideoClubBuilder().WithUser(user).WithUser(donor).Build();
        club.AddToWishlist(user, "rushmore");

        club.Donate(donor, "RUSHMORE");

        notifier.NotificationsFor(user).Should().ContainSingle();
    }

    [Fact]
    public void Donating_a_wishlisted_title_notifies_the_wishlisting_user()
    {
        var user = new UserBuilder().Build();
        var donor = new UserBuilder().Named("Donor").WithEmail("d@example.com").Build();
        var (club, notifier, _) = new VideoClubBuilder().WithUser(user).WithUser(donor).Build();
        club.AddToWishlist(user, "Rushmore");

        club.Donate(donor, "Rushmore");

        notifier.NotificationsFor(user).Should().ContainSingle()
            .Which.Message.Should().Contain("available");
    }
}
