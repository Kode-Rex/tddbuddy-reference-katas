import { describe, expect, it } from 'vitest';
import { CopyStatus } from '../src/CopyStatus.js';
import { Isbn } from '../src/Isbn.js';
import { LOAN_PERIOD_DAYS } from '../src/Loan.js';
import { BookBuilder } from './BookBuilder.js';
import { LibraryBuilder } from './LibraryBuilder.js';
import { MemberBuilder } from './MemberBuilder.js';

const DAY_0 = new Date(Date.UTC(2026, 0, 1));
const ISBN = '978-0134757599';
const MS_PER_DAY = 86_400_000;

describe('Checkouts', () => {
  it('checking out an available copy marks the copy as checked out', () => {
    const member = new MemberBuilder().build();
    const { library } = new LibraryBuilder().openedOn(DAY_0).withMember(member)
      .withBook(new BookBuilder().withIsbn(ISBN).withCopies(1)).build();
    const loan = library.checkOut(member, new Isbn(ISBN));
    expect(loan.copy.status).toBe(CopyStatus.CheckedOut);
  });

  it('checking out an available copy creates a loan with a due date fourteen days from today', () => {
    const member = new MemberBuilder().build();
    const { library } = new LibraryBuilder().openedOn(DAY_0).withMember(member)
      .withBook(new BookBuilder().withIsbn(ISBN).withCopies(1)).build();
    const loan = library.checkOut(member, new Isbn(ISBN));
    expect(loan.borrowedOn.getTime()).toBe(DAY_0.getTime());
    expect(loan.dueOn.getTime()).toBe(DAY_0.getTime() + LOAN_PERIOD_DAYS * MS_PER_DAY);
  });

  it('checking out when no copy is available is rejected', () => {
    const member = new MemberBuilder().build();
    const other = new MemberBuilder().named('Other').build();
    const { library } = new LibraryBuilder().openedOn(DAY_0)
      .withMember(member).withMember(other)
      .withBook(new BookBuilder().withIsbn(ISBN).withCopies(1)).build();
    library.checkOut(other, new Isbn(ISBN));
    expect(() => library.checkOut(member, new Isbn(ISBN))).toThrow();
  });
});
