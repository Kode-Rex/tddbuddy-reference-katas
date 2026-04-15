import { Age } from '../src/Age.js';
import { User } from '../src/User.js';

export class UserBuilder {
  private _name = 'Alex Member';
  private _email = 'alex@example.com';
  private _age = new Age(30);
  private _isAdmin = false;
  private _priorityPoints = 0;

  named(name: string): this { this._name = name; return this; }
  withEmail(email: string): this { this._email = email; return this; }
  aged(years: number): this { this._age = new Age(years); return this; }
  asAdmin(): this { this._isAdmin = true; return this; }
  withPriorityPoints(points: number): this { this._priorityPoints = points; return this; }

  build(): User {
    const user = new User(this._name, this._email, this._age, this._isAdmin);
    if (this._priorityPoints > 0) user.seedPriorityPoints(this._priorityPoints);
    return user;
  }
}
