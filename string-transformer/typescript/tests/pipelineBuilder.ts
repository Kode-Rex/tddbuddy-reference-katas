import {
  Capitalise,
  CamelCase,
  Pipeline,
  Repeat,
  Replace,
  Reverse,
  RemoveWhitespace,
  SnakeCase,
  Truncate,
  type Transformation,
} from '../src/pipeline.js';

export class PipelineBuilder {
  private readonly steps: Transformation[] = [];

  capitalise(): this { this.steps.push(new Capitalise()); return this; }
  reverse(): this { this.steps.push(new Reverse()); return this; }
  removeWhitespace(): this { this.steps.push(new RemoveWhitespace()); return this; }
  snakeCase(): this { this.steps.push(new SnakeCase()); return this; }
  camelCase(): this { this.steps.push(new CamelCase()); return this; }
  truncate(n: number): this { this.steps.push(new Truncate(n)); return this; }
  repeat(n: number): this { this.steps.push(new Repeat(n)); return this; }
  replace(target: string, replacement: string): this {
    this.steps.push(new Replace(target, replacement));
    return this;
  }

  build(): Pipeline {
    return new Pipeline([...this.steps]);
  }
}
