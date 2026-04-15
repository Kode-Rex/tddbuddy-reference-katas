import { describe, expect, it } from 'vitest';
import { CopyStatus } from '../src/CopyStatus.js';
import { Isbn } from '../src/Isbn.js';
import { LOAN_PERIOD_DAYS } from '../src/Loan.js';
import { Money } from '../src/Money.js';
import { BookBuilder } from './BookBuilder.js';
import { LibraryBuilder } from './LibraryBuilder.js';
import { MemberBuilder } from './MemberBuilder.js';

const DAY_0 = new Date(Date.UTC(2026, 0, 1));
const ISBN = '978-0134757599';

function openLibraryWithActiveLoan() {
  const member = new MemberBuilder().build();
  const { library, notifier, clock } = new LibraryBuilder().openedOn(DAY_0).withMember(member)
    .withBook(new BookBuilder().withIsbn(ISBN).withCopies(1)).build();
  library.checkOut(member, new Isbn(ISBN));
  return { library, notifier, clock, member };
}

describe('Returns', () => {
  it('returning a checked-out copy marks the copy as available', () => {
    const { library, clock, member } = openLibraryWithActiveLoan();
    clock.advanceDays(5);
    library.returnCopy(member, new Isbn(ISBN));
    expect(library.books[0]!.copies[0]!.status).toBe(CopyStatus.Available);
  });

  it('returning a copy closes the loan', () => {
    const { library, clock, member } = openLibraryWithActiveLoan();
    clock.advanceDays(5);
    library.returnCopy(member, new Isbn(ISBN));
    expect(library.loans).toHaveLength(0);
  });

  it('returning on time incurs no fine', () => {
    const { library, clock, member } = openLibraryWithActiveLoan();
    clock.advanceDays(LOAN_PERIOD_DAYS);
    const fine = library.returnCopy(member, new Isbn(ISBN));
    expect(fine.equals(Money.zero)).toBe(true);
  });

  it('returning one day late incurs a ten pence fine', () => {
    const { library, clock, member } = openLibraryWithActiveLoan();
    clock.advanceDays(LOAN_PERIOD_DAYS + 1);
    const fine = library.returnCopy(member, new Isbn(ISBN));
    expect(fine.equals(new Money(0.10))).toBe(true);
  });

  it('returning ten days late incurs a one pound fine', () => {
    const { library, clock, member } = openLibraryWithActiveLoan();
    clock.advanceDays(LOAN_PERIOD_DAYS + 10);
    const fine = library.returnCopy(member, new Isbn(ISBN));
    expect(fine.equals(new Money(1.00))).toBe(true);
  });
});
