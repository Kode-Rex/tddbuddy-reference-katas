export class CacheCapacityInvalidError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'CacheCapacityInvalidError';
  }
}

export class CacheTtlInvalidError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'CacheTtlInvalidError';
  }
}
