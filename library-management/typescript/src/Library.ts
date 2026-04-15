import { Book } from './Book.js';
import type { Clock } from './Clock.js';
import { BookNotInCatalogError, NoActiveLoanError, NoCopiesAvailableError } from './errors.js';
import { Isbn } from './Isbn.js';
import { Loan } from './Loan.js';
import { Member } from './Member.js';
import { Money } from './Money.js';
import type { Notifier } from './Notifier.js';
import { Reservation } from './Reservation.js';

const reservationAvailable = (title: string) => `'${title}' is now available to borrow`;
const reservationExpired = (title: string) => `Your reservation for '${title}' has expired`;

export class Library {
  private readonly _books = new Map<string, Book>();
  private readonly _members: Member[] = [];
  private readonly _loans: Loan[] = [];
  private readonly _reservations: Reservation[] = [];
  private _nextMemberId = 1;

  constructor(private readonly clock: Clock, private readonly notifier: Notifier) {}

  get books(): readonly Book[] { return [...this._books.values()]; }
  get members(): readonly Member[] { return this._members; }
  get loans(): readonly Loan[] { return this._loans; }
  get reservations(): readonly Reservation[] { return this._reservations; }

  addBook(title: string, author: string, isbn: Isbn): Book {
    const existing = this._books.get(isbn.value);
    if (existing) {
      existing.addCopy();
      return existing;
    }
    const book = new Book(title, author, isbn);
    book.addCopy();
    this._books.set(isbn.value, book);
    return book;
  }

  addCopyOf(isbn: Isbn): Book {
    const book = this.requireBook(isbn);
    book.addCopy();
    return book;
  }

  removeCopy(isbn: Isbn): void {
    const book = this.requireBook(isbn);
    book.removeOneCopy();
    if (book.copyCount === 0) this._books.delete(isbn.value);
  }

  register(name: string): Member {
    const member = new Member(this._nextMemberId++, name);
    this._members.push(member);
    return member;
  }

  seedMember(member: Member): void {
    this._members.push(member);
    if (member.id >= this._nextMemberId) this._nextMemberId = member.id + 1;
  }

  seedBook(book: Book, copyCount: number): Book {
    this._books.set(book.isbn.value, book);
    for (let i = 0; i < copyCount; i++) book.addCopy();
    return book;
  }

  checkOut(member: Member, isbn: Isbn): Loan {
    const book = this.requireBook(isbn);
    const today = this.clock.today();

    const reservedCopy = book.findReservedCopy();
    if (reservedCopy) {
      const head = this.headReservationFor(isbn);
      if (!head || head.member !== member) {
        throw new NoCopiesAvailableError(`No copies of '${isbn}' are available`);
      }
      this.removeReservation(head);
      reservedCopy.markCheckedOut();
      const reservedLoan = new Loan(member, reservedCopy, today);
      this._loans.push(reservedLoan);
      return reservedLoan;
    }

    const available = book.findAvailableCopy();
    if (!available) throw new NoCopiesAvailableError(`No copies of '${isbn}' are available`);
    available.markCheckedOut();
    const loan = new Loan(member, available, today);
    this._loans.push(loan);
    return loan;
  }

  returnCopy(member: Member, isbn: Isbn): Money {
    const idx = this._loans.findIndex(
      (l) => !l.isClosed && l.member === member && l.copy.isbn.equals(isbn),
    );
    if (idx === -1) throw new NoActiveLoanError(`Member has no active loan of '${isbn}'`);
    const loan = this._loans[idx]!;

    const today = this.clock.today();
    loan.close(today);
    this._loans.splice(idx, 1);
    const fine = loan.fineFor(today);

    const head = this.headReservationFor(isbn);
    if (head) {
      loan.copy.markReserved();
      head.markNotified(today);
      this.notifier.send(head.member, reservationAvailable(this.bookTitle(isbn)));
    } else {
      loan.copy.markAvailable();
    }

    return fine;
  }

  reserve(member: Member, isbn: Isbn): void {
    this.requireBook(isbn);
    this._reservations.push(new Reservation(member, isbn, this.clock.today()));
  }

  expireReservations(): void {
    const today = this.clock.today();
    const expired = this._reservations.filter((r) => r.hasExpiredAt(today));
    for (const reservation of expired) {
      this.removeReservation(reservation);
      this.notifier.send(
        reservation.member,
        reservationExpired(this.bookTitle(reservation.isbn)),
      );

      const book = this._books.get(reservation.isbn.value);
      const reservedCopy = book?.findReservedCopy();
      if (!reservedCopy) continue;

      const next = this.headReservationFor(reservation.isbn);
      if (next) {
        next.markNotified(today);
        this.notifier.send(next.member, reservationAvailable(this.bookTitle(reservation.isbn)));
      } else {
        reservedCopy.markAvailable();
      }
    }
  }

  reservationsFor(isbn: Isbn): Reservation[] {
    return this._reservations.filter((r) => r.isbn.equals(isbn));
  }

  private headReservationFor(isbn: Isbn): Reservation | undefined {
    return this._reservations.find((r) => r.isbn.equals(isbn));
  }

  private removeReservation(reservation: Reservation): void {
    const idx = this._reservations.indexOf(reservation);
    if (idx !== -1) this._reservations.splice(idx, 1);
  }

  private requireBook(isbn: Isbn): Book {
    const book = this._books.get(isbn.value);
    if (!book) throw new BookNotInCatalogError(`Book '${isbn}' is not in the catalog`);
    return book;
  }

  private bookTitle(isbn: Isbn): string {
    return this._books.get(isbn.value)?.title ?? isbn.value;
  }
}
