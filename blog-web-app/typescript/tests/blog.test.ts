import { describe, expect, it } from 'vitest';
import { UnauthorizedOperationError } from '../src/UnauthorizedOperationError.js';
import { BlogBuilder } from './BlogBuilder.js';

const T0 = new Date(Date.UTC(2026, 0, 15, 9, 0, 0));

describe('Blog', () => {
  // --- Post Creation ---

  it('new blog has no posts', () => {
    const { blog } = new BlogBuilder().build();
    expect(blog.posts.size).toBe(0);
  });

  it('creating a post auto-registers the user', () => {
    const { blog } = new BlogBuilder()
      .withPost('Alice', 'Hello', 'World')
      .build();
    expect(blog.users.has('Alice')).toBe(true);
  });

  it('a post records title, body, author, and timestamp', () => {
    const { posts } = new BlogBuilder()
      .startingAt(T0)
      .withPost('Alice', 'My Title', 'My Body', 0)
      .build();
    const post = posts[0]!;
    expect(post.title).toBe('My Title');
    expect(post.body).toBe('My Body');
    expect(post.author).toBe('Alice');
    expect(post.timestamp).toEqual(T0);
  });

  it('a user can create multiple posts', () => {
    const { blog } = new BlogBuilder()
      .withPost('Alice', 'First', 'Body 1', 0)
      .withPost('Alice', 'Second', 'Body 2', 1)
      .build();
    expect(blog.posts.size).toBe(2);
  });

  it('each post receives a unique id', () => {
    const { posts } = new BlogBuilder()
      .withPost('Alice', 'First', 'Body 1', 0)
      .withPost('Alice', 'Second', 'Body 2', 1)
      .build();
    expect(posts[0]!.id).not.toBe(posts[1]!.id);
  });

  // --- Editing Posts ---

  it('a user can edit their own post', () => {
    const { blog, posts } = new BlogBuilder()
      .withPost('Alice', 'Original', 'Original body')
      .build();
    blog.editPost('Alice', posts[0]!.id, 'Updated', 'Updated body');
    expect(posts[0]!.title).toBe('Updated');
  });

  it('editing updates both title and body', () => {
    const { blog, posts } = new BlogBuilder()
      .withPost('Alice', 'Original', 'Original body')
      .build();
    blog.editPost('Alice', posts[0]!.id, 'New Title', 'New Body');
    expect(posts[0]!.title).toBe('New Title');
    expect(posts[0]!.body).toBe('New Body');
  });

  it("editing another user's post throws unauthorized", () => {
    const { blog, posts } = new BlogBuilder()
      .withPost('Alice', "Alice's Post", 'Body')
      .withPost('Bob', "Bob's Post", 'Body', 1)
      .build();
    expect(() => blog.editPost('Bob', posts[0]!.id, 'Hacked', 'Hacked')).toThrow(
      UnauthorizedOperationError,
    );
    expect(() => blog.editPost('Bob', posts[0]!.id, 'Hacked', 'Hacked')).toThrow(
      "User 'Bob' is not the author of post '1'",
    );
  });

  // --- Deleting Posts ---

  it('a user can delete their own post', () => {
    const { blog, posts } = new BlogBuilder()
      .withPost('Alice', 'To Delete', 'Body')
      .build();
    blog.deletePost('Alice', posts[0]!.id);
    expect(blog.posts.size).toBe(0);
  });

  it("deleting another user's post throws unauthorized", () => {
    const { blog, posts } = new BlogBuilder()
      .withPost('Alice', "Alice's Post", 'Body')
      .withPost('Bob', "Bob's Post", 'Body', 1)
      .build();
    expect(() => blog.deletePost('Bob', posts[0]!.id)).toThrow(UnauthorizedOperationError);
    expect(() => blog.deletePost('Bob', posts[0]!.id)).toThrow(
      "User 'Bob' is not the author of post '1'",
    );
  });

  it('deleting a post removes its comments', () => {
    const { blog, posts } = new BlogBuilder()
      .withPost('Alice', 'Post', 'Body')
      .withComment(0, 'Bob', 'Nice post!', 1)
      .build();
    blog.deletePost('Alice', posts[0]!.id);
    expect(blog.posts.size).toBe(0);
  });

  // --- Comments ---

  it('any user can comment on any post', () => {
    const { posts } = new BlogBuilder()
      .withPost('Alice', 'Post', 'Body')
      .withComment(0, 'Bob', 'Great post!', 1)
      .build();
    expect(posts[0]!.comments).toHaveLength(1);
    expect(posts[0]!.comments[0]!.body).toBe('Great post!');
  });

  it('a comment records author, body, and timestamp', () => {
    const { posts } = new BlogBuilder()
      .startingAt(T0)
      .withPost('Alice', 'Post', 'Body', 0)
      .withComment(0, 'Bob', 'Nice!', 5)
      .build();
    const comment = posts[0]!.comments[0]!;
    expect(comment.author).toBe('Bob');
    expect(comment.body).toBe('Nice!');
    expect(comment.timestamp).toEqual(new Date(T0.getTime() + 5 * 60_000));
  });

  it('commenting auto-registers the user', () => {
    const { blog } = new BlogBuilder()
      .withPost('Alice', 'Post', 'Body')
      .withComment(0, 'Charlie', 'Hello!', 1)
      .build();
    expect(blog.users.has('Charlie')).toBe(true);
  });

  it('a user can delete their own comment', () => {
    const { blog, posts } = new BlogBuilder()
      .withPost('Alice', 'Post', 'Body')
      .withComment(0, 'Bob', 'To delete', 1)
      .build();
    const commentId = posts[0]!.comments[0]!.id;
    blog.deleteComment('Bob', posts[0]!.id, commentId);
    expect(posts[0]!.comments).toHaveLength(0);
  });

  it("deleting another user's comment throws unauthorized", () => {
    const { blog, posts } = new BlogBuilder()
      .withPost('Alice', 'Post', 'Body')
      .withComment(0, 'Bob', "Bob's comment", 1)
      .build();
    const commentId = posts[0]!.comments[0]!.id;
    expect(() => blog.deleteComment('Alice', posts[0]!.id, commentId)).toThrow(
      UnauthorizedOperationError,
    );
    expect(() => blog.deleteComment('Alice', posts[0]!.id, commentId)).toThrow(
      `User 'Alice' is not the author of comment '${commentId}'`,
    );
  });

  // --- Tags ---

  it('a post author can add a tag to their own post', () => {
    const { posts } = new BlogBuilder()
      .withPost('Alice', 'TDD Post', 'Body')
      .withTag(0, 'TDD')
      .build();
    expect(posts[0]!.tags.has('TDD')).toBe(true);
  });

  it("adding a tag to another user's post throws unauthorized", () => {
    const { blog, posts } = new BlogBuilder()
      .withPost('Alice', 'Post', 'Body')
      .withPost('Bob', "Bob's Post", 'Body', 1)
      .build();
    expect(() => blog.addTag('Bob', posts[0]!.id, 'hack')).toThrow(UnauthorizedOperationError);
    expect(() => blog.addTag('Bob', posts[0]!.id, 'hack')).toThrow(
      "User 'Bob' is not the author of post '1'",
    );
  });

  it('adding the same tag twice is idempotent', () => {
    const { posts } = new BlogBuilder()
      .withPost('Alice', 'Post', 'Body')
      .withTag(0, 'TDD')
      .withTag(0, 'TDD')
      .build();
    expect(posts[0]!.tags.size).toBe(1);
  });

  // --- Queries ---

  it('recent posts returns the N most recent posts', () => {
    const { blog } = new BlogBuilder()
      .withPost('Alice', 'First', 'Body', 0)
      .withPost('Bob', 'Second', 'Body', 5)
      .withPost('Alice', 'Third', 'Body', 10)
      .withPost('Bob', 'Fourth', 'Body', 15)
      .withPost('Alice', 'Fifth', 'Body', 20)
      .build();
    const recent = blog.recentPosts(3);
    expect(recent).toHaveLength(3);
    expect(recent[0]!.title).toBe('Fifth');
    expect(recent[1]!.title).toBe('Fourth');
    expect(recent[2]!.title).toBe('Third');
  });

  it('recent posts returns fewer than N when not enough posts exist', () => {
    const { blog } = new BlogBuilder()
      .withPost('Alice', 'Only', 'Body')
      .build();
    const recent = blog.recentPosts(5);
    expect(recent).toHaveLength(1);
  });

  it('posts by tag returns matching posts most recent first', () => {
    const { blog } = new BlogBuilder()
      .withPost('Alice', 'First TDD', 'Body', 0)
      .withPost('Bob', 'Second TDD', 'Body', 5)
      .withPost('Alice', 'No Tag', 'Body', 10)
      .withTag(0, 'TDD')
      .withTag(1, 'TDD')
      .build();
    const results = blog.postsByTag('TDD');
    expect(results).toHaveLength(2);
    expect(results[0]!.title).toBe('Second TDD');
    expect(results[1]!.title).toBe('First TDD');
  });

  it('posts by tag returns empty when no posts match', () => {
    const { blog } = new BlogBuilder()
      .withPost('Alice', 'Post', 'Body')
      .build();
    expect(blog.postsByTag('nonexistent')).toEqual([]);
  });

  it('all tags for user returns distinct tags across their posts', () => {
    const { blog } = new BlogBuilder()
      .withPost('Alice', 'Post 1', 'Body', 0)
      .withPost('Alice', 'Post 2', 'Body', 1)
      .withTag(0, 'TDD')
      .withTag(0, 'C#')
      .withTag(1, 'TDD')
      .withTag(1, 'Testing')
      .build();
    const tags = blog.allTagsForUser('Alice');
    expect(tags.size).toBe(3);
    expect(tags.has('TDD')).toBe(true);
    expect(tags.has('C#')).toBe(true);
    expect(tags.has('Testing')).toBe(true);
  });

  it('all tags for user returns empty when user has no tags', () => {
    const { blog } = new BlogBuilder()
      .withPost('Alice', 'Post', 'Body')
      .build();
    expect(blog.allTagsForUser('Alice').size).toBe(0);
  });
});
