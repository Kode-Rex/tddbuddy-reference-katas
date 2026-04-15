import { describe, expect, it } from 'vitest';
import { Money } from '../src/Money.js';
import { RENTAL_PERIOD_DAYS } from '../src/Rental.js';
import { TitleBuilder } from './TitleBuilder.js';
import { UserBuilder } from './UserBuilder.js';
import { VideoClubBuilder } from './VideoClubBuilder.js';

const DAY_0 = new Date(Date.UTC(2026, 0, 1));

describe('Returns', () => {
  it('on-time return awards two priority points', () => {
    const user = new UserBuilder().build();
    const { club, clock } = new VideoClubBuilder()
      .openedOn(DAY_0).withUser(user)
      .withTitle(new TitleBuilder().named('Jaws').build()).build();
    club.rent(user, 'Jaws');
    clock.advanceDays(RENTAL_PERIOD_DAYS);
    club.returnTitle(user, 'Jaws');
    expect(user.priorityPoints).toBe(2);
  });

  it('late return deducts two priority points', () => {
    const user = new UserBuilder().withPriorityPoints(4).build();
    const { club, clock } = new VideoClubBuilder()
      .openedOn(DAY_0).withUser(user)
      .withTitle(new TitleBuilder().named('Jaws').build()).build();
    club.rent(user, 'Jaws');
    clock.advanceDays(RENTAL_PERIOD_DAYS + 1);
    club.returnTitle(user, 'Jaws');
    expect(user.priorityPoints).toBe(2);
  });

  it('late return dispatches a late alert', () => {
    const user = new UserBuilder().build();
    const { club, notifier, clock } = new VideoClubBuilder()
      .openedOn(DAY_0).withUser(user)
      .withTitle(new TitleBuilder().named('Jaws').build()).build();
    club.rent(user, 'Jaws');
    clock.advanceDays(RENTAL_PERIOD_DAYS + 3);
    club.returnTitle(user, 'Jaws');
    const notes = notifier.notificationsFor(user);
    expect(notes).toHaveLength(1);
    expect(notes[0]!.message).toContain('overdue');
  });

  it('user with an overdue rental cannot rent another title', () => {
    const user = new UserBuilder().build();
    const { club, clock } = new VideoClubBuilder()
      .openedOn(DAY_0).withUser(user)
      .withTitle(new TitleBuilder().named('Jaws').build())
      .withTitle(new TitleBuilder().named('Casablanca').build()).build();
    club.rent(user, 'Jaws');
    clock.advanceDays(RENTAL_PERIOD_DAYS + 1);
    club.markOverdueRentals();
    expect(() => club.rent(user, 'Casablanca')).toThrow();
  });

  it('returning the overdue title unblocks renting', () => {
    const user = new UserBuilder().build();
    const { club, clock } = new VideoClubBuilder()
      .openedOn(DAY_0).withUser(user)
      .withTitle(new TitleBuilder().named('Jaws').build())
      .withTitle(new TitleBuilder().named('Casablanca').build()).build();
    club.rent(user, 'Jaws');
    clock.advanceDays(RENTAL_PERIOD_DAYS + 1);
    club.markOverdueRentals();
    club.returnTitle(user, 'Jaws');
    expect(club.rent(user, 'Casablanca').equals(new Money(2.50))).toBe(true);
  });
});
