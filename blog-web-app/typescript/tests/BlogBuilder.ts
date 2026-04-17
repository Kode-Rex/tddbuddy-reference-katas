import { Blog } from '../src/Blog.js';
import { Post } from '../src/Post.js';
import { FixedClock } from './FixedClock.js';

interface SeededPost {
  user: string;
  title: string;
  body: string;
  minutesAfterStart: number;
}

interface SeededComment {
  postIndex: number;
  user: string;
  body: string;
  minutesAfterStart: number;
}

interface SeededTag {
  postIndex: number;
  tag: string;
}

export class BlogBuilder {
  private _startTime = new Date(Date.UTC(2026, 0, 15, 9, 0, 0));
  private _seededPosts: SeededPost[] = [];
  private _seededComments: SeededComment[] = [];
  private _seededTags: SeededTag[] = [];

  startingAt(date: Date): this {
    this._startTime = date;
    return this;
  }

  withPost(user: string, title: string, body: string, minutesAfterStart = 0): this {
    this._seededPosts.push({ user, title, body, minutesAfterStart });
    return this;
  }

  withComment(postIndex: number, user: string, body: string, minutesAfterStart = 0): this {
    this._seededComments.push({ postIndex, user, body, minutesAfterStart });
    return this;
  }

  withTag(postIndex: number, tag: string): this {
    this._seededTags.push({ postIndex, tag });
    return this;
  }

  build(): { blog: Blog; clock: FixedClock; posts: Post[] } {
    const clock = new FixedClock(this._startTime);
    const blog = new Blog(clock);
    const posts: Post[] = [];

    for (const { user, title, body, minutesAfterStart } of this._seededPosts) {
      clock.advanceTo(new Date(this._startTime.getTime() + minutesAfterStart * 60_000));
      posts.push(blog.createPost(user, title, body));
    }

    for (const { postIndex, user, body, minutesAfterStart } of this._seededComments) {
      clock.advanceTo(new Date(this._startTime.getTime() + minutesAfterStart * 60_000));
      blog.addComment(user, posts[postIndex]!.id, body);
    }

    for (const { postIndex, tag } of this._seededTags) {
      blog.addTag(posts[postIndex]!.author, posts[postIndex]!.id, tag);
    }

    return { blog, clock, posts };
  }
}
