import {
  Basket,
  BookOutOfRangeError,
  MAX_BOOK_ID,
  MIN_BOOK_ID,
} from '../src/basket.js';

// Test-folder fluent synthesiser. Place copies of books into the basket by id.
// Reads as the basket literal under test.
export class BasketBuilder {
  private readonly counts: number[] = new Array<number>(MAX_BOOK_ID + 1).fill(0);

  withBook(series: number, count: number): this {
    if (series < MIN_BOOK_ID || series > MAX_BOOK_ID) {
      throw new BookOutOfRangeError();
    }
    if (count < 0) count = 0;
    this.counts[series]! += count;
    return this;
  }

  build(): Basket {
    return new Basket(this.counts);
  }
}
