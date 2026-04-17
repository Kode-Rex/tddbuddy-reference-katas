import { Category } from '../src/Category.js';
import { ExpenseItem } from '../src/ExpenseItem.js';
import { Money } from '../src/Money.js';

export class ExpenseItemBuilder {
  private _description = 'Office supplies';
  private _amount = 25;
  private _category: Category = Category.Other;

  withDescription(description: string): this { this._description = description; return this; }
  withAmount(amount: number): this { this._amount = amount; return this; }
  withCategory(category: Category): this { this._category = category; return this; }

  asMeal(amount = 30): this {
    return this.withCategory(Category.Meals).withAmount(amount).withDescription('Team lunch');
  }

  asTravel(amount = 200): this {
    return this.withCategory(Category.Travel).withAmount(amount).withDescription('Flight');
  }

  asAccommodation(amount = 150): this {
    return this.withCategory(Category.Accommodation).withAmount(amount).withDescription('Hotel stay');
  }

  asEquipment(amount = 800): this {
    return this.withCategory(Category.Equipment).withAmount(amount).withDescription('Laptop');
  }

  build(): ExpenseItem {
    return new ExpenseItem(this._description, new Money(this._amount), this._category);
  }
}
