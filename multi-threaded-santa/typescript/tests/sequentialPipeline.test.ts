import { Pipeline } from '../src/Pipeline.js';
import { Present } from '../src/Present.js';

describe('Sequential pipeline', () => {
  it('a single present flows through all four stages', async () => {
    const pipeline = new Pipeline(10, 10, 10);
    const present = new Present(1);

    await pipeline.processSequentially([present]);

    expect(present.state).toBe('delivered');
    expect(pipeline.delivered).toHaveLength(1);
    expect(pipeline.delivered[0]!.id).toBe(1);
  });

  it('multiple presents all complete the full pipeline', async () => {
    const pipeline = new Pipeline(10, 10, 10);
    const presents = Array.from({ length: 10 }, (_, i) => new Present(i + 1));

    await pipeline.processSequentially(presents);

    expect(presents.every((p) => p.state === 'delivered')).toBe(true);
    expect(pipeline.delivered).toHaveLength(10);
  });

  it('presents emerge from the pipeline in order', async () => {
    const pipeline = new Pipeline(10, 10, 10);
    const presents = Array.from({ length: 5 }, (_, i) => new Present(i + 1));

    await pipeline.processSequentially(presents);

    expect(pipeline.delivered.map((p) => p.id)).toEqual([1, 2, 3, 4, 5]);
  });
});
