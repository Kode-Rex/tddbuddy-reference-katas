import type { Title } from './Title.js';
import type { User } from './User.js';

export const RENTAL_PERIOD_DAYS = 15;

const MS_PER_DAY = 86_400_000;

export class Rental {
  public readonly dueOn: Date;

  constructor(
    public readonly user: User,
    public readonly title: Title,
    public readonly rentedOn: Date,
  ) {
    this.dueOn = new Date(rentedOn.getTime() + RENTAL_PERIOD_DAYS * MS_PER_DAY);
  }

  isLateAt(returnDate: Date): boolean {
    return returnDate.getTime() > this.dueOn.getTime();
  }
}
