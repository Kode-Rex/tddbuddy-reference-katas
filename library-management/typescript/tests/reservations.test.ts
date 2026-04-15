import { describe, expect, it } from 'vitest';
import { CopyStatus } from '../src/CopyStatus.js';
import { Isbn } from '../src/Isbn.js';
import { RESERVATION_EXPIRY_DAYS } from '../src/Reservation.js';
import { BookBuilder } from './BookBuilder.js';
import { LibraryBuilder } from './LibraryBuilder.js';
import { MemberBuilder } from './MemberBuilder.js';

const DAY_0 = new Date(Date.UTC(2026, 0, 1));
const ISBN = '978-0134757599';

describe('Reservations', () => {
  it('reserving an unavailable book adds the member to the queue', () => {
    const borrower = new MemberBuilder().named('Borrower').build();
    const reserver = new MemberBuilder().named('Reserver').build();
    const { library } = new LibraryBuilder().openedOn(DAY_0)
      .withMember(borrower).withMember(reserver)
      .withBook(new BookBuilder().withIsbn(ISBN).withCopies(1)).build();
    library.checkOut(borrower, new Isbn(ISBN));

    library.reserve(reserver, new Isbn(ISBN));

    const queue = library.reservationsFor(new Isbn(ISBN));
    expect(queue).toHaveLength(1);
    expect(queue[0]!.member).toBe(reserver);
  });

  it('returning a copy with a non-empty queue marks the copy as reserved', () => {
    const borrower = new MemberBuilder().named('Borrower').build();
    const reserver = new MemberBuilder().named('Reserver').build();
    const { library, clock } = new LibraryBuilder().openedOn(DAY_0)
      .withMember(borrower).withMember(reserver)
      .withBook(new BookBuilder().withIsbn(ISBN).withCopies(1)).build();
    library.checkOut(borrower, new Isbn(ISBN));
    library.reserve(reserver, new Isbn(ISBN));
    clock.advanceDays(2);

    library.returnCopy(borrower, new Isbn(ISBN));

    expect(library.books[0]!.copies[0]!.status).toBe(CopyStatus.Reserved);
  });

  it('returning a copy with a non-empty queue notifies the head of the queue', () => {
    const borrower = new MemberBuilder().named('Borrower').build();
    const reserver = new MemberBuilder().named('Reserver').build();
    const { library, notifier, clock } = new LibraryBuilder().openedOn(DAY_0)
      .withMember(borrower).withMember(reserver)
      .withBook(new BookBuilder().titled('Refactoring').withIsbn(ISBN).withCopies(1)).build();
    library.checkOut(borrower, new Isbn(ISBN));
    library.reserve(reserver, new Isbn(ISBN));
    clock.advanceDays(2);

    library.returnCopy(borrower, new Isbn(ISBN));

    const notes = notifier.availabilityNotificationsFor(reserver);
    expect(notes).toHaveLength(1);
    expect(notes[0]!.message).toContain('Refactoring');
  });

  it('reservations older than three days expire and the next reserver is notified', () => {
    const borrower = new MemberBuilder().named('Borrower').build();
    const first = new MemberBuilder().named('First').build();
    const second = new MemberBuilder().named('Second').build();
    const { library, notifier, clock } = new LibraryBuilder().openedOn(DAY_0)
      .withMember(borrower).withMember(first).withMember(second)
      .withBook(new BookBuilder().titled('Refactoring').withIsbn(ISBN).withCopies(1)).build();
    library.checkOut(borrower, new Isbn(ISBN));
    library.reserve(first, new Isbn(ISBN));
    library.reserve(second, new Isbn(ISBN));
    library.returnCopy(borrower, new Isbn(ISBN));

    clock.advanceDays(RESERVATION_EXPIRY_DAYS + 1);
    library.expireReservations();

    expect(notifier.expirationNotificationsFor(first)).toHaveLength(1);
    expect(notifier.availabilityNotificationsFor(second)).toHaveLength(1);
  });

  it('checking out a reserved copy satisfies the reservation and clears it', () => {
    const borrower = new MemberBuilder().named('Borrower').build();
    const reserver = new MemberBuilder().named('Reserver').build();
    const { library, clock } = new LibraryBuilder().openedOn(DAY_0)
      .withMember(borrower).withMember(reserver)
      .withBook(new BookBuilder().withIsbn(ISBN).withCopies(1)).build();
    library.checkOut(borrower, new Isbn(ISBN));
    library.reserve(reserver, new Isbn(ISBN));
    library.returnCopy(borrower, new Isbn(ISBN));
    clock.advanceDays(1);

    const loan = library.checkOut(reserver, new Isbn(ISBN));

    expect(loan.copy.status).toBe(CopyStatus.CheckedOut);
    expect(library.reservationsFor(new Isbn(ISBN))).toHaveLength(0);
  });
});
