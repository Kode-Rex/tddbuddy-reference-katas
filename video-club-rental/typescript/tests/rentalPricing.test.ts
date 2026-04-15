import { describe, expect, it } from 'vitest';
import { Money } from '../src/Money.js';
import { TitleBuilder } from './TitleBuilder.js';
import { UserBuilder } from './UserBuilder.js';
import { VideoClubBuilder } from './VideoClubBuilder.js';

function stockedClub() {
  const user = new UserBuilder().build();
  const { club } = new VideoClubBuilder()
    .withUser(user)
    .withTitle(new TitleBuilder().named('The Godfather').build())
    .withTitle(new TitleBuilder().named('Casablanca').build())
    .withTitle(new TitleBuilder().named('Jaws').build())
    .build();
  return { club, user };
}

describe('Rental Pricing', () => {
  it('first simultaneous rental costs two pounds fifty', () => {
    const { club, user } = stockedClub();
    expect(club.rent(user, 'The Godfather').equals(new Money(2.50))).toBe(true);
  });

  it('second simultaneous rental costs two pounds twenty-five', () => {
    const { club, user } = stockedClub();
    club.rent(user, 'The Godfather');
    expect(club.rent(user, 'Casablanca').equals(new Money(2.25))).toBe(true);
  });

  it('third simultaneous rental costs one pound seventy-five', () => {
    const { club, user } = stockedClub();
    club.rent(user, 'The Godfather');
    club.rent(user, 'Casablanca');
    expect(club.rent(user, 'Jaws').equals(new Money(1.75))).toBe(true);
  });

  it('renting two titles charges four pounds seventy-five total', () => {
    const { club, user } = stockedClub();
    const first = club.rent(user, 'The Godfather');
    const second = club.rent(user, 'Casablanca');
    expect(first.plus(second).equals(new Money(4.75))).toBe(true);
  });

  it('renting three titles charges six pounds fifty total', () => {
    const { club, user } = stockedClub();
    const first = club.rent(user, 'The Godfather');
    const second = club.rent(user, 'Casablanca');
    const third = club.rent(user, 'Jaws');
    expect(first.plus(second).plus(third).equals(new Money(6.50))).toBe(true);
  });
});
