import { Book } from '../src/Book.js';
import { Isbn } from '../src/Isbn.js';

export class BookBuilder {
  private _title = 'The Pragmatic Programmer';
  private _author = 'Andrew Hunt';
  private _isbn = new Isbn('978-0201616224');
  private _copies = 1;

  titled(title: string): this { this._title = title; return this; }
  by(author: string): this { this._author = author; return this; }
  withIsbn(isbn: string): this { this._isbn = new Isbn(isbn); return this; }
  withCopies(copies: number): this { this._copies = copies; return this; }

  build(): Book { return new Book(this._title, this._author, this._isbn); }

  get copyCount(): number { return this._copies; }
}
