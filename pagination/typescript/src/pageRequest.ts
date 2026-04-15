export const DEFAULT_PAGE_NUMBER = 1;
export const DEFAULT_PAGE_SIZE = 10;
export const DEFAULT_TOTAL_ITEMS = 0;

export interface PageRequest {
  readonly pageNumber: number;
  readonly pageSize: number;
  readonly totalItems: number;
  readonly totalPages: number;
  readonly startItem: number;
  readonly endItem: number;
  readonly hasPrevious: boolean;
  readonly hasNext: boolean;
  pageWindow(windowSize: number): readonly number[];
}

export function createPageRequest(spec: {
  pageNumber: number;
  pageSize: number;
  totalItems: number;
}): PageRequest {
  if (spec.totalItems < 0) throw new Error('totalItems must be >= 0');
  if (spec.pageSize < 1) throw new Error('pageSize must be >= 1');

  const totalPages = spec.totalItems === 0
    ? 0
    : Math.ceil(spec.totalItems / spec.pageSize);

  const pageNumber = clampPageNumber(spec.pageNumber, totalPages);

  const startItem = spec.totalItems === 0 ? 0 : (pageNumber - 1) * spec.pageSize + 1;
  const endItem = spec.totalItems === 0
    ? 0
    : Math.min(pageNumber * spec.pageSize, spec.totalItems);

  return {
    pageNumber,
    pageSize: spec.pageSize,
    totalItems: spec.totalItems,
    totalPages,
    startItem,
    endItem,
    hasPrevious: pageNumber > 1,
    hasNext: pageNumber < totalPages,
    pageWindow(windowSize: number): readonly number[] {
      if (windowSize <= 0 || totalPages === 0) return [];
      const size = Math.min(windowSize, totalPages);
      const half = Math.floor(size / 2);
      let start = pageNumber - half;
      let end = start + size - 1;
      if (start < 1) { start = 1; end = size; }
      if (end > totalPages) { end = totalPages; start = end - size + 1; }
      return Array.from({ length: size }, (_, i) => start + i);
    },
  };
}

function clampPageNumber(requested: number, totalPages: number): number {
  if (totalPages === 0) return 1;
  if (requested < 1) return 1;
  if (requested > totalPages) return totalPages;
  return requested;
}
