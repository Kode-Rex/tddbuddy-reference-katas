import { describe, it, expect } from 'vitest';
import { PageRequestBuilder } from './pageRequestBuilder.js';

describe('PageRequest', () => {
  it('first page of 100 items with page size 10 shows items 1 through 10', () => {
    const request = new PageRequestBuilder().totalItems(100).pageSize(10).pageNumber(1).build();
    expect(request.startItem).toBe(1);
    expect(request.endItem).toBe(10);
    expect(request.hasPrevious).toBe(false);
    expect(request.hasNext).toBe(true);
    expect(request.totalPages).toBe(10);
  });

  it('middle page of 100 items with page size 10 shows items 41 through 50', () => {
    const request = new PageRequestBuilder().totalItems(100).pageSize(10).pageNumber(5).build();
    expect(request.startItem).toBe(41);
    expect(request.endItem).toBe(50);
    expect(request.hasPrevious).toBe(true);
    expect(request.hasNext).toBe(true);
  });

  it('last page of 100 items with page size 10 shows items 91 through 100', () => {
    const request = new PageRequestBuilder().totalItems(100).pageSize(10).pageNumber(10).build();
    expect(request.startItem).toBe(91);
    expect(request.endItem).toBe(100);
    expect(request.hasPrevious).toBe(true);
    expect(request.hasNext).toBe(false);
  });

  it('last page with a partial window shows only the remaining items', () => {
    const request = new PageRequestBuilder().totalItems(95).pageSize(10).pageNumber(10).build();
    expect(request.startItem).toBe(91);
    expect(request.endItem).toBe(95);
    expect(request.totalPages).toBe(10);
  });

  it('single item on one page reports itself as start and end', () => {
    const request = new PageRequestBuilder().totalItems(1).pageSize(10).pageNumber(1).build();
    expect(request.startItem).toBe(1);
    expect(request.endItem).toBe(1);
    expect(request.hasPrevious).toBe(false);
    expect(request.hasNext).toBe(false);
    expect(request.totalPages).toBe(1);
  });

  it('zero items reports no pages and no items', () => {
    const request = new PageRequestBuilder().totalItems(0).pageSize(10).pageNumber(1).build();
    expect(request.totalPages).toBe(0);
    expect(request.startItem).toBe(0);
    expect(request.endItem).toBe(0);
    expect(request.hasPrevious).toBe(false);
    expect(request.hasNext).toBe(false);
  });

  it('page number below 1 is clamped to 1', () => {
    const request = new PageRequestBuilder().totalItems(100).pageSize(10).pageNumber(0).build();
    expect(request.pageNumber).toBe(1);
    expect(request.startItem).toBe(1);
    expect(request.endItem).toBe(10);
  });

  it('page number above totalPages is clamped to the last page', () => {
    const request = new PageRequestBuilder().totalItems(100).pageSize(10).pageNumber(99).build();
    expect(request.pageNumber).toBe(10);
    expect(request.startItem).toBe(91);
    expect(request.endItem).toBe(100);
  });

  it('page size of 1 gives every item its own page', () => {
    const request = new PageRequestBuilder().totalItems(5).pageSize(1).pageNumber(3).build();
    expect(request.startItem).toBe(3);
    expect(request.endItem).toBe(3);
    expect(request.totalPages).toBe(5);
    expect(request.hasPrevious).toBe(true);
    expect(request.hasNext).toBe(true);
  });

  it('negative totalItems is rejected at construction', () => {
    expect(() => new PageRequestBuilder().totalItems(-1).build()).toThrow('totalItems must be >= 0');
  });

  it('page size below 1 is rejected at construction', () => {
    expect(() => new PageRequestBuilder().pageSize(0).build()).toThrow('pageSize must be >= 1');
  });

  it('pageWindow centers on the current page when there is room', () => {
    const first = new PageRequestBuilder().totalItems(100).pageSize(10).pageNumber(1).build();
    const middle = new PageRequestBuilder().totalItems(100).pageSize(10).pageNumber(5).build();
    const last = new PageRequestBuilder().totalItems(100).pageSize(10).pageNumber(10).build();

    expect(first.pageWindow(5)).toEqual([1, 2, 3, 4, 5]);
    expect(middle.pageWindow(5)).toEqual([3, 4, 5, 6, 7]);
    expect(last.pageWindow(5)).toEqual([6, 7, 8, 9, 10]);
  });

  it('pageWindow is clipped when totalPages is smaller than the window', () => {
    const request = new PageRequestBuilder().totalItems(40).pageSize(10).pageNumber(3).build();
    expect(request.pageWindow(5)).toEqual([1, 2, 3, 4]);
  });

  it('pageWindow on an empty dataset is empty', () => {
    const request = new PageRequestBuilder().totalItems(0).pageSize(10).pageNumber(1).build();
    expect(request.pageWindow(5)).toEqual([]);
  });
});
