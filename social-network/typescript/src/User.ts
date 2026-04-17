export class User {
  readonly name: string;
  private readonly _following = new Set<string>();

  constructor(name: string) {
    this.name = name;
  }

  get following(): ReadonlySet<string> {
    return this._following;
  }

  follow(userName: string): boolean {
    if (userName === this.name) return false;
    if (this._following.has(userName)) return false;
    this._following.add(userName);
    return true;
  }
}
