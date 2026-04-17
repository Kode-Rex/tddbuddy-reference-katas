import { Money } from './Money.js';

export class Product {
  private _stock: number;

  constructor(
    public readonly name: string,
    public readonly price: Money,
    public readonly purchaseCode: string,
    stock: number,
  ) {
    this._stock = stock;
  }

  get stock(): number {
    return this._stock;
  }

  get isInStock(): boolean {
    return this._stock > 0;
  }

  decrementStock(): void {
    this._stock--;
  }
}
