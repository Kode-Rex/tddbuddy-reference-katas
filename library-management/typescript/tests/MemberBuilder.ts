import { Member } from '../src/Member.js';

let nextId = 1000;

export class MemberBuilder {
  private _id = ++nextId;
  private _name = 'Alex Reader';

  named(name: string): this { this._name = name; return this; }
  withId(id: number): this { this._id = id; return this; }

  build(): Member { return new Member(this._id, this._name); }
}
