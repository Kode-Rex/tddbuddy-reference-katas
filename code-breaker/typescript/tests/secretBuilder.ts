import { createSecret, Peg, type Secret } from '../src/codeBreaker.js';

export class SecretBuilder {
  private _pegs: readonly [Peg, Peg, Peg, Peg] = [Peg.One, Peg.Two, Peg.Three, Peg.Four];

  with(a: Peg, b: Peg, c: Peg, d: Peg): this {
    this._pegs = [a, b, c, d];
    return this;
  }

  build(): Secret {
    return createSecret(this._pegs);
  }
}
