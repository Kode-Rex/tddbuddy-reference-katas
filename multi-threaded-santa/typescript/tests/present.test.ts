import { Present } from '../src/Present.js';

describe('Present lifecycle', () => {
  it('a new present starts in the unmade state', () => {
    const present = new Present(1);

    expect(present.state).toBe('unmade');
  });

  it('making a present transitions it to the made state', () => {
    const present = new Present(1);

    present.make();

    expect(present.state).toBe('made');
  });

  it('wrapping a made present transitions it to the wrapped state', () => {
    const present = new Present(1);
    present.make();

    present.wrap();

    expect(present.state).toBe('wrapped');
  });

  it('loading a wrapped present transitions it to the loaded state', () => {
    const present = new Present(1);
    present.make();
    present.wrap();

    present.load();

    expect(present.state).toBe('loaded');
  });

  it('delivering a loaded present transitions it to the delivered state', () => {
    const present = new Present(1);
    present.make();
    present.wrap();
    present.load();

    present.deliver();

    expect(present.state).toBe('delivered');
  });
});
