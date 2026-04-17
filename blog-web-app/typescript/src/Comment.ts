export class Comment {
  constructor(
    readonly id: number,
    readonly author: string,
    readonly body: string,
    readonly timestamp: Date,
  ) {}
}
