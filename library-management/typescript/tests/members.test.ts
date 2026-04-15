import { describe, expect, it } from 'vitest';
import { LibraryBuilder } from './LibraryBuilder.js';

const DAY_0 = new Date(Date.UTC(2026, 0, 1));

describe('Members', () => {
  it('new library has no members', () => {
    const { library } = new LibraryBuilder().openedOn(DAY_0).build();
    expect(library.members).toHaveLength(0);
  });

  it('registering a member adds them to the library', () => {
    const { library } = new LibraryBuilder().openedOn(DAY_0).build();
    const member = library.register('Ada Lovelace');
    expect(library.members).toHaveLength(1);
    expect(library.members[0]).toBe(member);
    expect(member.name).toBe('Ada Lovelace');
  });
});
