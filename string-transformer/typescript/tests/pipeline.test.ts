import { describe, it, expect } from 'vitest';
import { PipelineBuilder } from './pipelineBuilder.js';

describe('Pipeline', () => {
  it('empty pipeline returns the input unchanged', () => {
    const pipeline = new PipelineBuilder().build();
    expect(pipeline.run('hello world')).toBe('hello world');
  });

  it('capitalise capitalises each word', () => {
    const pipeline = new PipelineBuilder().capitalise().build();
    expect(pipeline.run('hello world')).toBe('Hello World');
  });

  it('reverse reverses the whole string', () => {
    const pipeline = new PipelineBuilder().reverse().build();
    expect(pipeline.run('hello world')).toBe('dlrow olleh');
  });

  it('removeWhitespace drops every space', () => {
    const pipeline = new PipelineBuilder().removeWhitespace().build();
    expect(pipeline.run('hello world')).toBe('helloworld');
  });

  it('snakeCase lowercases and joins with underscores', () => {
    const pipeline = new PipelineBuilder().snakeCase().build();
    expect(pipeline.run('hello world')).toBe('hello_world');
  });

  it('snakeCase collapses hyphens and whitespace into single underscores', () => {
    const pipeline = new PipelineBuilder().snakeCase().build();
    expect(pipeline.run('hello-world test')).toBe('hello_world_test');
  });

  it('camelCase lowercases the first word and title-cases the rest', () => {
    const pipeline = new PipelineBuilder().camelCase().build();
    expect(pipeline.run('Hello World')).toBe('helloWorld');
  });

  it('camelCase normalises all-uppercase input', () => {
    const pipeline = new PipelineBuilder().camelCase().build();
    expect(pipeline.run('HELLO WORLD')).toBe('helloWorld');
  });

  it('truncate shortens long input and appends the marker', () => {
    const pipeline = new PipelineBuilder().truncate(5).build();
    expect(pipeline.run('hello world')).toBe('hello...');
  });

  it('truncate leaves short input untouched', () => {
    const pipeline = new PipelineBuilder().truncate(50).build();
    expect(pipeline.run('hello world')).toBe('hello world');
  });

  it('repeat produces n space-joined copies', () => {
    const pipeline = new PipelineBuilder().repeat(3).build();
    expect(pipeline.run('ha')).toBe('ha ha ha');
  });

  it('replace swaps every occurrence of the target', () => {
    const pipeline = new PipelineBuilder().replace('world', 'there').build();
    expect(pipeline.run('hello world')).toBe('hello there');
  });

  it('chaining applies transformations in order', () => {
    const pipeline = new PipelineBuilder().capitalise().reverse().build();
    expect(pipeline.run('hello world')).toBe('dlroW olleH');
  });

  it('chaining snakeCase then capitalise uppercases letters after underscores', () => {
    const pipeline = new PipelineBuilder().snakeCase().capitalise().build();
    expect(pipeline.run('hello world')).toBe('Hello_World');
  });

  it('empty input survives capitalise', () => {
    const pipeline = new PipelineBuilder().capitalise().build();
    expect(pipeline.run('')).toBe('');
  });
});
