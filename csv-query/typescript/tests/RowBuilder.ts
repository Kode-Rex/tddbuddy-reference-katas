import { Row } from '../src/Row.js';

export class RowBuilder {
  private name = 'Alice';
  private age = '35';
  private city = 'London';
  private salary = '75000';

  withName(name: string): this { this.name = name; return this; }
  withAge(age: string): this { this.age = age; return this; }
  withCity(city: string): this { this.city = city; return this; }
  withSalary(salary: string): this { this.salary = salary; return this; }

  build(): Row {
    return new Row({ name: this.name, age: this.age, city: this.city, salary: this.salary });
  }
}
