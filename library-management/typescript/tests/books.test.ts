import { describe, expect, it } from 'vitest';
import { CopyStatus } from '../src/CopyStatus.js';
import { Isbn } from '../src/Isbn.js';
import { BookBuilder } from './BookBuilder.js';
import { LibraryBuilder } from './LibraryBuilder.js';

const DAY_0 = new Date(Date.UTC(2026, 0, 1));

describe('Books and Copies', () => {
  it('new library has no books', () => {
    const { library } = new LibraryBuilder().openedOn(DAY_0).build();
    expect(library.books).toHaveLength(0);
  });

  it('adding a book with one copy makes the book available', () => {
    const { library } = new LibraryBuilder().openedOn(DAY_0).build();
    const book = library.addBook('Refactoring', 'Martin Fowler', new Isbn('978-0134757599'));
    expect(library.books).toHaveLength(1);
    expect(book.copyCount).toBe(1);
    expect(book.copies[0]!.status).toBe(CopyStatus.Available);
  });

  it('adding another copy of an existing book increments the copy count', () => {
    const { library } = new LibraryBuilder().openedOn(DAY_0).build();
    const isbn = new Isbn('978-0134757599');
    library.addBook('Refactoring', 'Martin Fowler', isbn);
    library.addCopyOf(isbn);
    expect(library.books[0]!.copyCount).toBe(2);
  });

  it('removing a copy decrements the copy count', () => {
    const { library } = new LibraryBuilder().openedOn(DAY_0)
      .withBook(new BookBuilder().withIsbn('978-0134757599').withCopies(3)).build();
    library.removeCopy(new Isbn('978-0134757599'));
    expect(library.books[0]!.copyCount).toBe(2);
  });

  it('removing the last copy removes the book from the catalog', () => {
    const { library } = new LibraryBuilder().openedOn(DAY_0)
      .withBook(new BookBuilder().withIsbn('978-0134757599').withCopies(1)).build();
    library.removeCopy(new Isbn('978-0134757599'));
    expect(library.books).toHaveLength(0);
  });
});
