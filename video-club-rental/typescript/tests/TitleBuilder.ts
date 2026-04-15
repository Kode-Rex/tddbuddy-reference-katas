import { Title } from '../src/Title.js';

export class TitleBuilder {
  private _name = 'The Godfather';
  private _copies = 3;

  named(name: string): this { this._name = name; return this; }
  withCopies(copies: number): this { this._copies = copies; return this; }

  build(): Title { return new Title(this._name, this._copies); }
}
