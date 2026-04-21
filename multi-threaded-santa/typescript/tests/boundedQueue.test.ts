import { BoundedQueue } from '../src/BoundedQueue.js';

describe('Bounded queue', () => {
  it('enqueuing an item makes it available for dequeue', async () => {
    const queue = new BoundedQueue<number>(10);

    await queue.enqueue(42);
    const result = await queue.dequeue();

    expect(result.success).toBe(true);
    if (result.success) {
      expect(result.item).toBe(42);
    }
  });

  it('dequeuing returns items in FIFO order', async () => {
    const queue = new BoundedQueue<number>(10);

    await queue.enqueue(1);
    await queue.enqueue(2);
    await queue.enqueue(3);

    const first = await queue.dequeue();
    const second = await queue.dequeue();
    const third = await queue.dequeue();

    expect(first.success && first.item).toBe(1);
    expect(second.success && second.item).toBe(2);
    expect(third.success && third.item).toBe(3);
  });

  it('a queue reports its current count', async () => {
    const queue = new BoundedQueue<number>(10);

    expect(queue.count).toBe(0);

    await queue.enqueue(1);
    await queue.enqueue(2);

    expect(queue.count).toBe(2);

    await queue.dequeue();

    expect(queue.count).toBe(1);
  });

  it('a full queue rejects further enqueues until space is available', () => {
    const queue = new BoundedQueue<number>(2);

    expect(queue.tryEnqueue(1)).toBe(true);
    expect(queue.tryEnqueue(2)).toBe(true);
    expect(queue.tryEnqueue(3)).toBe(false);

    expect(queue.count).toBe(2);
  });
});
