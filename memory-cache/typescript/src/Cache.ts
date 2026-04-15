import type { Clock } from './Clock.js';
import { CacheCapacityInvalidError, CacheTtlInvalidError } from './errors.js';

export const DEFAULT_CAPACITY = 100;
export const DEFAULT_TTL_MS = 60_000;

interface Entry<V> {
  key: string;
  value: V;
  insertedAt: number; // epoch ms
}

interface Node<V> {
  entry: Entry<V>;
  prev: Node<V> | null;
  next: Node<V> | null;
}

export class Cache<V> {
  private readonly capacity: number;
  private readonly ttlMs: number;
  private readonly clock: Clock;

  // Doubly-linked list: head = most recently used, tail = least recently used.
  private head: Node<V> | null = null;
  private tail: Node<V> | null = null;
  private readonly index = new Map<string, Node<V>>();

  constructor(capacity: number, ttlMs: number, clock: Clock) {
    if (capacity <= 0) {
      throw new CacheCapacityInvalidError('Capacity must be positive');
    }
    if (ttlMs <= 0) {
      throw new CacheTtlInvalidError('TTL must be positive');
    }
    this.capacity = capacity;
    this.ttlMs = ttlMs;
    this.clock = clock;
  }

  get size(): number {
    return this.index.size;
  }

  put(key: string, value: V): void {
    const existing = this.index.get(key);
    const nowMs = this.clock.now().getTime();

    if (existing) {
      existing.entry = { key, value, insertedAt: nowMs };
      this.unlink(existing);
      this.prepend(existing);
      return;
    }

    if (this.index.size >= this.capacity) {
      const lru = this.tail;
      if (lru) {
        this.unlink(lru);
        this.index.delete(lru.entry.key);
      }
    }

    const node: Node<V> = {
      entry: { key, value, insertedAt: nowMs },
      prev: null,
      next: null,
    };
    this.prepend(node);
    this.index.set(key, node);
  }

  get(key: string): V | null {
    const node = this.index.get(key);
    if (!node) return null;

    if (this.isExpired(node.entry)) {
      this.unlink(node);
      this.index.delete(key);
      return null;
    }

    this.unlink(node);
    this.prepend(node);
    return node.entry.value;
  }

  contains(key: string): boolean {
    const node = this.index.get(key);
    if (!node) return false;
    return !this.isExpired(node.entry);
  }

  evictExpired(): void {
    let node = this.head;
    while (node) {
      const next = node.next;
      if (this.isExpired(node.entry)) {
        this.unlink(node);
        this.index.delete(node.entry.key);
      }
      node = next;
    }
  }

  private isExpired(entry: Entry<V>): boolean {
    return this.clock.now().getTime() - entry.insertedAt >= this.ttlMs;
  }

  private prepend(node: Node<V>): void {
    node.prev = null;
    node.next = this.head;
    if (this.head) this.head.prev = node;
    this.head = node;
    if (!this.tail) this.tail = node;
  }

  private unlink(node: Node<V>): void {
    if (node.prev) node.prev.next = node.next;
    else this.head = node.next;
    if (node.next) node.next.prev = node.prev;
    else this.tail = node.prev;
    node.prev = null;
    node.next = null;
  }
}
