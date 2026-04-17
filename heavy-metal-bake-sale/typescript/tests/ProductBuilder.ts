import { Money } from '../src/Money.js';
import { Product } from '../src/Product.js';

export class ProductBuilder {
  private _name = 'Brownie';
  private _price = 0.75;
  private _purchaseCode = 'B';
  private _stock = 48;

  withName(name: string): this { this._name = name; return this; }
  withPrice(price: number): this { this._price = price; return this; }
  withPurchaseCode(code: string): this { this._purchaseCode = code; return this; }
  withStock(stock: number): this { this._stock = stock; return this; }

  build(): Product {
    return new Product(this._name, new Money(this._price), this._purchaseCode, this._stock);
  }
}
