import { describe, expect, it } from 'vitest';
import { NetworkBuilder } from './NetworkBuilder.js';

const T0 = new Date(Date.UTC(2026, 0, 15, 9, 0, 0));

describe('Network', () => {
  // --- Registration ---

  it('new network has no users', () => {
    const { network } = new NetworkBuilder().build();
    expect(network.users.size).toBe(0);
  });

  it('posting auto-registers a new user', () => {
    const { network } = new NetworkBuilder()
      .withPost('Alice', 'Hello!')
      .build();
    expect(network.users.has('Alice')).toBe(true);
  });

  it('posting with an existing user does not duplicate registration', () => {
    const { network } = new NetworkBuilder()
      .withPost('Alice', 'Hello!', 0)
      .withPost('Alice', 'Again!', 1)
      .build();
    expect(network.users.size).toBe(1);
  });

  // --- Posting ---

  it('a user can post a message', () => {
    const { network } = new NetworkBuilder()
      .withPost('Alice', 'What a wonderfully sunny day!')
      .build();
    const timeline = network.timeline('Alice');
    expect(timeline).toHaveLength(1);
    expect(timeline[0]!.content).toBe('What a wonderfully sunny day!');
  });

  it('a post records the content and timestamp from the clock', () => {
    const { network } = new NetworkBuilder()
      .startingAt(T0)
      .withPost('Alice', 'Hello!', 0)
      .build();
    const post = network.timeline('Alice')[0]!;
    expect(post.author).toBe('Alice');
    expect(post.content).toBe('Hello!');
    expect(post.timestamp).toEqual(T0);
  });

  it('a user can post multiple messages', () => {
    const { network } = new NetworkBuilder()
      .withPost('Alice', 'First', 0)
      .withPost('Alice', 'Second', 1)
      .build();
    expect(network.timeline('Alice')).toHaveLength(2);
  });

  // --- Timeline ---

  it('timeline of a user with no posts is empty', () => {
    const { network } = new NetworkBuilder().build();
    expect(network.timeline('Alice')).toEqual([]);
  });

  it('timeline returns the user\'s own posts', () => {
    const { network } = new NetworkBuilder()
      .withPost('Alice', 'Sunny day!')
      .build();
    const timeline = network.timeline('Alice');
    expect(timeline).toHaveLength(1);
    expect(timeline[0]!.content).toBe('Sunny day!');
  });

  it('timeline returns posts in reverse chronological order', () => {
    const { network } = new NetworkBuilder()
      .withPost('Alice', 'First', 0)
      .withPost('Alice', 'Second', 5)
      .withPost('Alice', 'Third', 10)
      .build();
    const timeline = network.timeline('Alice');
    expect(timeline[0]!.content).toBe('Third');
    expect(timeline[1]!.content).toBe('Second');
    expect(timeline[2]!.content).toBe('First');
  });

  it('timeline does not include posts from other users', () => {
    const { network } = new NetworkBuilder()
      .withPost('Alice', "Alice's post", 0)
      .withPost('Bob', "Bob's post", 1)
      .build();
    const timeline = network.timeline('Alice');
    expect(timeline).toHaveLength(1);
    expect(timeline[0]!.author).toBe('Alice');
  });

  // --- Following ---

  it('a user can follow another user', () => {
    const { network } = new NetworkBuilder()
      .withPost('Alice', 'Hello!', 0)
      .withPost('Charlie', 'Hi!', 1)
      .withFollow('Charlie', 'Alice')
      .build();
    expect(network.users.get('Charlie')!.following.has('Alice')).toBe(true);
  });

  it('following is idempotent', () => {
    const { network } = new NetworkBuilder()
      .withPost('Alice', 'Hello!', 0)
      .withPost('Charlie', 'Hi!', 1)
      .withFollow('Charlie', 'Alice')
      .withFollow('Charlie', 'Alice')
      .build();
    expect(network.users.get('Charlie')!.following.size).toBe(1);
  });

  it('a user cannot follow themselves', () => {
    const { network } = new NetworkBuilder()
      .withPost('Alice', 'Hello!')
      .build();
    network.follow('Alice', 'Alice');
    expect(network.users.get('Alice')!.following.size).toBe(0);
  });

  // --- Wall ---

  it('wall of a user with no posts and no follows is empty', () => {
    const { network } = new NetworkBuilder().build();
    expect(network.wall('Alice')).toEqual([]);
  });

  it('wall shows the user\'s own posts', () => {
    const { network } = new NetworkBuilder()
      .withPost('Alice', 'My post')
      .build();
    const wall = network.wall('Alice');
    expect(wall).toHaveLength(1);
    expect(wall[0]!.content).toBe('My post');
  });

  it('wall includes posts from followed users', () => {
    const { network } = new NetworkBuilder()
      .withPost('Alice', "Alice's post", 0)
      .withPost('Bob', "Bob's post", 1)
      .withPost('Charlie', "Charlie's post", 2)
      .withFollow('Charlie', 'Alice')
      .withFollow('Charlie', 'Bob')
      .build();
    const wall = network.wall('Charlie');
    expect(wall).toHaveLength(3);
    const authors = wall.map((p) => p.author);
    expect(authors).toContain('Alice');
    expect(authors).toContain('Bob');
    expect(authors).toContain('Charlie');
  });

  it('wall returns posts in reverse chronological order across all authors', () => {
    const { network } = new NetworkBuilder()
      .withPost('Alice', 'Alice early', 0)
      .withPost('Bob', 'Bob middle', 5)
      .withPost('Charlie', 'Charlie late', 10)
      .withFollow('Charlie', 'Alice')
      .withFollow('Charlie', 'Bob')
      .build();
    const wall = network.wall('Charlie');
    expect(wall[0]!.content).toBe('Charlie late');
    expect(wall[1]!.content).toBe('Bob middle');
    expect(wall[2]!.content).toBe('Alice early');
  });

  it('wall does not include posts from users not followed', () => {
    const { network } = new NetworkBuilder()
      .withPost('Alice', "Alice's post", 0)
      .withPost('Bob', "Bob's post", 1)
      .withPost('Charlie', "Charlie's post", 2)
      .withFollow('Charlie', 'Alice')
      .build();
    const wall = network.wall('Charlie');
    const authors = wall.map((p) => p.author);
    expect(authors).not.toContain('Bob');
  });
});
