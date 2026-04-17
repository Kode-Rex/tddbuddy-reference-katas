export class Post {
  constructor(
    readonly author: string,
    readonly content: string,
    readonly timestamp: Date,
  ) {}
}
