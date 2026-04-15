import { describe, expect, it } from 'vitest';
import { RENTAL_PERIOD_DAYS } from '../src/Rental.js';
import { TitleBuilder } from './TitleBuilder.js';
import { UserBuilder } from './UserBuilder.js';
import { VideoClubBuilder } from './VideoClubBuilder.js';

describe('Priority Access', () => {
  it('user with five priority points has priority access to new releases', () => {
    const user = new UserBuilder().withPriorityPoints(5).build();
    const { club } = new VideoClubBuilder().withUser(user).build();
    expect(club.hasPriorityAccess(user)).toBe(true);
  });

  it('user with four priority points does not have priority access', () => {
    const user = new UserBuilder().withPriorityPoints(4).build();
    const { club } = new VideoClubBuilder().withUser(user).build();
    expect(club.hasPriorityAccess(user)).toBe(false);
  });

  it('priority points cannot go below zero', () => {
    const user = new UserBuilder().withPriorityPoints(1).build();
    const { club, clock } = new VideoClubBuilder()
      .withUser(user)
      .withTitle(new TitleBuilder().named('Jaws').build()).build();
    club.rent(user, 'Jaws');
    clock.advanceDays(RENTAL_PERIOD_DAYS + 1);
    club.returnTitle(user, 'Jaws');
    expect(user.priorityPoints).toBe(0);
  });
});
