import { describe, expect, it } from 'vitest';
import { TitleBuilder } from './TitleBuilder.js';
import { UserBuilder } from './UserBuilder.js';
import { VideoClubBuilder } from './VideoClubBuilder.js';

describe('Donations', () => {
  it('donating a new title creates a library entry with one copy', () => {
    const donor = new UserBuilder().build();
    const { club } = new VideoClubBuilder().withUser(donor).build();
    club.donate(donor, 'Rushmore');
    const rushmore = club.titles.find((t) => t.name === 'Rushmore');
    expect(rushmore).toBeDefined();
    expect(rushmore!.totalCopies).toBe(1);
  });

  it('donating an existing title increments its copy count', () => {
    const donor = new UserBuilder().build();
    const { club } = new VideoClubBuilder()
      .withUser(donor)
      .withTitle(new TitleBuilder().named('Jaws').withCopies(2).build())
      .build();
    club.donate(donor, 'Jaws');
    const jaws = club.titles.find((t) => t.name === 'Jaws')!;
    expect(jaws.totalCopies).toBe(3);
    expect(jaws.availableCopies).toBe(3);
  });

  it('donating awards ten loyalty points to the donor', () => {
    const donor = new UserBuilder().build();
    const { club } = new VideoClubBuilder().withUser(donor).build();
    club.donate(donor, 'Rushmore');
    expect(donor.loyaltyPoints).toBe(10);
  });
});
