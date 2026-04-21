import { Pipeline } from '../src/Pipeline.js';
import { Present } from '../src/Present.js';

describe('Concurrent pipeline', () => {
  it('multiple workers can process a stage concurrently', async () => {
    const pipeline = new Pipeline(50, 50, 50);
    const presents = Array.from({ length: 20 }, (_, i) => new Present(i + 1));

    await pipeline.processConcurrently(presents, 3, 2, 2);

    expect(presents.every((p) => p.state === 'delivered')).toBe(true);
    expect(pipeline.delivered).toHaveLength(20);
  });

  it('the sleigh constraint allows only one delivery at a time', async () => {
    const pipeline = new Pipeline(50, 50, 50);
    const presents = Array.from({ length: 30 }, (_, i) => new Present(i + 1));

    await pipeline.processConcurrently(presents, 4, 3, 2);

    expect(pipeline.delivered).toHaveLength(30);
  });

  it('loading pauses while the sleigh is delivering', async () => {
    const pipeline = new Pipeline(20, 20, 20);
    const presents = Array.from({ length: 15 }, (_, i) => new Present(i + 1));

    await pipeline.processConcurrently(presents, 2, 2, 2);

    expect(presents.every((p) => p.state === 'delivered')).toBe(true);
  });

  it('all presents are delivered when the pipeline completes', async () => {
    const pipeline = new Pipeline(100, 100, 100);
    const presents = Array.from({ length: 50 }, (_, i) => new Present(i + 1));

    await pipeline.processConcurrently(presents, 4, 3, 2);

    expect(pipeline.delivered).toHaveLength(50);
    const deliveredIds = pipeline.delivered.map((p) => p.id).sort((a, b) => a - b);
    expect(deliveredIds).toEqual(Array.from({ length: 50 }, (_, i) => i + 1));
  });
});
