import { Comment } from './Comment.js';

export class Post {
  readonly id: number;
  private _title: string;
  private _body: string;
  readonly author: string;
  readonly timestamp: Date;
  private readonly _comments: Comment[] = [];
  private readonly _tags = new Set<string>();

  constructor(id: number, title: string, body: string, author: string, timestamp: Date) {
    this.id = id;
    this._title = title;
    this._body = body;
    this.author = author;
    this.timestamp = timestamp;
  }

  get title(): string {
    return this._title;
  }

  get body(): string {
    return this._body;
  }

  get comments(): readonly Comment[] {
    return this._comments;
  }

  get tags(): ReadonlySet<string> {
    return this._tags;
  }

  edit(title: string, body: string): void {
    this._title = title;
    this._body = body;
  }

  addTag(tag: string): void {
    this._tags.add(tag);
  }

  addComment(comment: Comment): void {
    this._comments.push(comment);
  }

  removeComment(commentId: number): void {
    const index = this._comments.findIndex((c) => c.id === commentId);
    if (index >= 0) {
      this._comments.splice(index, 1);
    }
  }
}
