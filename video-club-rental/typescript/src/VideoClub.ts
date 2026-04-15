import { Age, AGE_ADULT_MINIMUM } from './Age.js';
import type { Clock } from './Clock.js';
import { Money } from './Money.js';
import type { Notifier } from './Notifier.js';
import * as PricingPolicy from './PricingPolicy.js';
import { Rental } from './Rental.js';
import { Title } from './Title.js';
import { User } from './User.js';

export const PRIORITY_ACCESS_THRESHOLD = 5;
export const ON_TIME_RETURN_AWARD = 2;
export const LATE_RETURN_PENALTY = 2;
export const DONATION_LOYALTY_AWARD = 10;

const WELCOME_MESSAGE = 'Welcome to the video club';
const lateAlert = (title: string) => `Your rental of '${title}' is overdue`;
const wishlistAvailable = (title: string) => `'${title}' is now available`;

export class VideoClub {
  private readonly _users = new Map<string, User>();
  private readonly _titles = new Map<string, Title>();
  private readonly _rentals: Rental[] = [];

  constructor(private readonly clock: Clock, private readonly notifier: Notifier) {}

  get users(): readonly User[] { return [...this._users.values()]; }
  get titles(): readonly Title[] { return [...this._titles.values()]; }

  register(name: string, email: string, age: Age): User {
    if (!age.isAdult) {
      throw new Error(`User must be at least ${AGE_ADULT_MINIMUM} to register`);
    }
    const user = new User(name, email, age);
    this._users.set(email.toLowerCase(), user);
    this.notifier.send(user, WELCOME_MESSAGE);
    return user;
  }

  createUser(admin: User, name: string, email: string, age: Age): User {
    if (!admin.isAdmin) throw new Error('Only admin users may create other users');
    return this.register(name, email, age);
  }

  seedUser(user: User): void {
    this._users.set(user.email.toLowerCase(), user);
  }

  addTitle(title: Title): Title {
    this._titles.set(title.name.toLowerCase(), title);
    return title;
  }

  rent(user: User, titleName: string): Money {
    if (user.hasOverdue) throw new Error('User has an overdue rental and cannot rent');
    const title = this.requireTitle(titleName);
    const existing = this.activeRentalsFor(user).length;
    const cost = PricingPolicy.calculate(1, existing);
    title.checkOut();
    this._rentals.push(new Rental(user, title, this.clock.today()));
    return cost;
  }

  returnTitle(user: User, titleName: string): void {
    const idx = this._rentals.findIndex(
      (r) => r.user === user && r.title.name.toLowerCase() === titleName.toLowerCase(),
    );
    if (idx === -1) throw new Error(`User has no active rental of '${titleName}'`);
    const rental = this._rentals[idx]!;
    this._rentals.splice(idx, 1);
    rental.title.checkIn();

    const today = this.clock.today();
    if (rental.isLateAt(today)) {
      user.deductPriorityPoints(LATE_RETURN_PENALTY);
      this.notifier.send(user, lateAlert(rental.title.name));
    } else {
      user.awardPriorityPoints(ON_TIME_RETURN_AWARD);
    }

    if (!this.activeRentalsFor(user).some((r) => r.isLateAt(today))) {
      user.clearOverdue();
    }
  }

  markOverdueRentals(): void {
    const today = this.clock.today();
    for (const r of this._rentals) {
      if (r.isLateAt(today)) r.user.markOverdue();
    }
  }

  hasPriorityAccess(user: User): boolean {
    return user.priorityPoints >= PRIORITY_ACCESS_THRESHOLD;
  }

  addToWishlist(user: User, titleName: string): void {
    user.addWish(titleName);
  }

  donate(donor: User, titleName: string): void {
    const existing = this._titles.get(titleName.toLowerCase());
    if (existing) {
      existing.addCopy();
    } else {
      this.addTitle(new Title(titleName, 1));
    }
    donor.awardLoyaltyPoints(DONATION_LOYALTY_AWARD);
    for (const user of this._users.values()) {
      if (user.wishes(titleName)) {
        this.notifier.send(user, wishlistAvailable(titleName));
      }
    }
  }

  private requireTitle(titleName: string): Title {
    const title = this._titles.get(titleName.toLowerCase());
    if (!title) throw new Error(`Title '${titleName}' is not in the catalog`);
    return title;
  }

  private activeRentalsFor(user: User): Rental[] {
    return this._rentals.filter((r) => r.user === user);
  }
}
