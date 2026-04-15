import {
  createPageRequest,
  DEFAULT_PAGE_NUMBER,
  DEFAULT_PAGE_SIZE,
  DEFAULT_TOTAL_ITEMS,
  type PageRequest,
} from '../src/pageRequest.js';

export class PageRequestBuilder {
  private _pageNumber = DEFAULT_PAGE_NUMBER;
  private _pageSize = DEFAULT_PAGE_SIZE;
  private _totalItems = DEFAULT_TOTAL_ITEMS;

  pageNumber(n: number): this { this._pageNumber = n; return this; }
  pageSize(n: number): this { this._pageSize = n; return this; }
  totalItems(n: number): this { this._totalItems = n; return this; }

  build(): PageRequest {
    return createPageRequest({
      pageNumber: this._pageNumber,
      pageSize: this._pageSize,
      totalItems: this._totalItems,
    });
  }
}
