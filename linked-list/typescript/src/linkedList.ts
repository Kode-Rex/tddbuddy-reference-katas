class Node<T> {
  constructor(
    public value: T,
    public next: Node<T> | null = null,
  ) {}
}

export class LinkedList<T> {
  private head: Node<T> | null = null;
  private count = 0;

  size(): number {
    return this.count;
  }

  append(value: T): void {
    const node = new Node(value);
    if (this.head === null) {
      this.head = node;
    } else {
      let current = this.head;
      while (current.next !== null) current = current.next;
      current.next = node;
    }
    this.count++;
  }

  prepend(value: T): void {
    this.head = new Node(value, this.head);
    this.count++;
  }

  get(index: number): T {
    return this.nodeAt(index).value;
  }

  remove(index: number): T {
    if (index < 0 || index >= this.count) throw outOfRange(index);
    let removed: Node<T>;
    if (index === 0) {
      removed = this.head as Node<T>;
      this.head = removed.next;
    } else {
      const previous = this.nodeAt(index - 1);
      removed = previous.next as Node<T>;
      previous.next = removed.next;
    }
    this.count--;
    return removed.value;
  }

  insertAt(index: number, value: T): void {
    if (index < 0 || index > this.count) throw outOfRange(index);
    if (index === 0) {
      this.prepend(value);
      return;
    }
    if (index === this.count) {
      this.append(value);
      return;
    }
    const previous = this.nodeAt(index - 1);
    previous.next = new Node(value, previous.next);
    this.count++;
  }

  contains(value: T): boolean {
    return this.indexOf(value) >= 0;
  }

  indexOf(value: T): number {
    let i = 0;
    for (let current = this.head; current !== null; current = current.next, i++) {
      if (current.value === value) return i;
    }
    return -1;
  }

  toArray(): T[] {
    const result: T[] = [];
    for (let current = this.head; current !== null; current = current.next) {
      result.push(current.value);
    }
    return result;
  }

  private nodeAt(index: number): Node<T> {
    if (index < 0 || index >= this.count) throw outOfRange(index);
    let current = this.head as Node<T>;
    for (let i = 0; i < index; i++) current = current.next as Node<T>;
    return current;
  }
}

function outOfRange(index: number): RangeError {
  return new RangeError(`index out of range: ${index}`);
}
