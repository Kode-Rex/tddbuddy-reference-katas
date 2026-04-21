export type PresentState = 'unmade' | 'made' | 'wrapped' | 'loaded' | 'delivered';

export class Present {
  readonly id: number;
  private _state: PresentState = 'unmade';

  constructor(id: number) {
    this.id = id;
  }

  get state(): PresentState {
    return this._state;
  }

  make(): void {
    if (this._state !== 'unmade') {
      throw new Error(`Cannot make a present in state ${this._state}.`);
    }
    this._state = 'made';
  }

  wrap(): void {
    if (this._state !== 'made') {
      throw new Error(`Cannot wrap a present in state ${this._state}.`);
    }
    this._state = 'wrapped';
  }

  load(): void {
    if (this._state !== 'wrapped') {
      throw new Error(`Cannot load a present in state ${this._state}.`);
    }
    this._state = 'loaded';
  }

  deliver(): void {
    if (this._state !== 'loaded') {
      throw new Error(`Cannot deliver a present in state ${this._state}.`);
    }
    this._state = 'delivered';
  }
}
