import { describe, expect, it } from 'vitest';
import { UserBuilder } from './UserBuilder.js';
import { VideoClubBuilder } from './VideoClubBuilder.js';

describe('Wishlist', () => {
  it('user can add a title to their wishlist', () => {
    const user = new UserBuilder().build();
    const { club } = new VideoClubBuilder().withUser(user).build();
    club.addToWishlist(user, 'Rushmore');
    expect(user.wishes('Rushmore')).toBe(true);
  });

  it('wishlist matching is case-insensitive', () => {
    const user = new UserBuilder().build();
    const donor = new UserBuilder().named('Donor').withEmail('d@example.com').build();
    const { club, notifier } = new VideoClubBuilder().withUser(user).withUser(donor).build();
    club.addToWishlist(user, 'rushmore');
    club.donate(donor, 'RUSHMORE');
    expect(notifier.notificationsFor(user)).toHaveLength(1);
  });

  it('donating a wishlisted title notifies the wishlisting user', () => {
    const user = new UserBuilder().build();
    const donor = new UserBuilder().named('Donor').withEmail('d@example.com').build();
    const { club, notifier } = new VideoClubBuilder().withUser(user).withUser(donor).build();
    club.addToWishlist(user, 'Rushmore');
    club.donate(donor, 'Rushmore');
    const notes = notifier.notificationsFor(user);
    expect(notes).toHaveLength(1);
    expect(notes[0]!.message).toContain('available');
  });
});
