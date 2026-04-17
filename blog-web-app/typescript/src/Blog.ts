import type { Clock } from './Clock.js';
import { Comment } from './Comment.js';
import { Post } from './Post.js';
import { UnauthorizedOperationError } from './UnauthorizedOperationError.js';
import { User } from './User.js';

export class Blog {
  private readonly _users = new Map<string, User>();
  private readonly _posts = new Map<number, Post>();
  private _nextPostId = 1;
  private _nextCommentId = 1;

  constructor(private readonly clock: Clock) {}

  get users(): ReadonlyMap<string, User> {
    return this._users;
  }

  get posts(): ReadonlyMap<number, Post> {
    return this._posts;
  }

  createPost(userName: string, title: string, body: string): Post {
    this.ensureRegistered(userName);
    const post = new Post(this._nextPostId++, title, body, userName, this.clock.now());
    this._posts.set(post.id, post);
    return post;
  }

  editPost(userName: string, postId: number, title: string, body: string): void {
    const post = this.getPostOrThrow(postId);
    this.ensureAuthorOfPost(userName, post);
    post.edit(title, body);
  }

  deletePost(userName: string, postId: number): void {
    const post = this.getPostOrThrow(postId);
    this.ensureAuthorOfPost(userName, post);
    this._posts.delete(postId);
  }

  addComment(userName: string, postId: number, body: string): Comment {
    const post = this.getPostOrThrow(postId);
    this.ensureRegistered(userName);
    const comment = new Comment(this._nextCommentId++, userName, body, this.clock.now());
    post.addComment(comment);
    return comment;
  }

  deleteComment(userName: string, postId: number, commentId: number): void {
    const post = this.getPostOrThrow(postId);
    const comment = post.comments.find((c) => c.id === commentId);
    if (!comment) return;

    if (comment.author !== userName) {
      throw new UnauthorizedOperationError(
        `User '${userName}' is not the author of comment '${commentId}'`,
      );
    }

    post.removeComment(commentId);
  }

  addTag(userName: string, postId: number, tag: string): void {
    const post = this.getPostOrThrow(postId);
    this.ensureAuthorOfPost(userName, post);
    post.addTag(tag);
  }

  recentPosts(count: number): readonly Post[] {
    return [...this._posts.values()]
      .sort((a, b) => b.timestamp.getTime() - a.timestamp.getTime())
      .slice(0, count);
  }

  postsByTag(tag: string): readonly Post[] {
    return [...this._posts.values()]
      .filter((p) => p.tags.has(tag))
      .sort((a, b) => b.timestamp.getTime() - a.timestamp.getTime());
  }

  allTagsForUser(userName: string): ReadonlySet<string> {
    const tags = new Set<string>();
    for (const post of this._posts.values()) {
      if (post.author === userName) {
        for (const tag of post.tags) {
          tags.add(tag);
        }
      }
    }
    return tags;
  }

  private ensureRegistered(userName: string): void {
    if (!this._users.has(userName)) {
      this._users.set(userName, new User(userName));
    }
  }

  private getPostOrThrow(postId: number): Post {
    const post = this._posts.get(postId);
    if (!post) {
      throw new Error(`Post '${postId}' not found`);
    }
    return post;
  }

  private ensureAuthorOfPost(userName: string, post: Post): void {
    if (post.author !== userName) {
      throw new UnauthorizedOperationError(
        `User '${userName}' is not the author of post '${post.id}'`,
      );
    }
  }
}
