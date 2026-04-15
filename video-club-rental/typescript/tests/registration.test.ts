import { describe, expect, it } from 'vitest';
import { Age } from '../src/Age.js';
import { UserBuilder } from './UserBuilder.js';
import { VideoClubBuilder } from './VideoClubBuilder.js';

describe('Registration', () => {
  it('user aged eighteen registers successfully', () => {
    const { club } = new VideoClubBuilder().build();
    const user = club.register('Eighteen', 'eighteen@example.com', new Age(18));
    expect(user.age.isAdult).toBe(true);
    expect(club.users).toContain(user);
  });

  it('user aged seventeen is rejected as too young', () => {
    const { club } = new VideoClubBuilder().build();
    expect(() => club.register('Seventeen', 'seventeen@example.com', new Age(17))).toThrow();
  });

  it('registration dispatches a welcome email', () => {
    const { club, notifier } = new VideoClubBuilder().build();
    const user = club.register('Alex', 'alex@example.com', new Age(30));
    const notes = notifier.notificationsFor(user);
    expect(notes).toHaveLength(1);
    expect(notes[0]!.message).toContain('Welcome');
  });

  it('admin creates another user successfully', () => {
    const admin = new UserBuilder().named('Boss').withEmail('boss@example.com').asAdmin().build();
    const { club } = new VideoClubBuilder().withUser(admin).build();
    const created = club.createUser(admin, 'New Hire', 'new@example.com', new Age(22));
    expect(club.users).toContain(created);
  });

  it('non-admin attempting to create a user is rejected', () => {
    const regular = new UserBuilder().build();
    const { club } = new VideoClubBuilder().withUser(regular).build();
    expect(() => club.createUser(regular, 'New Hire', 'new@example.com', new Age(22))).toThrow();
  });
});
