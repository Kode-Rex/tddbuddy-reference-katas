// Byte-identical across C#, TypeScript, and Python. The truncation
// marker is part of the spec (see ../SCENARIOS.md).
export const TRUNCATION_MARKER = '...';

// Strategy. Every transformation is a self-contained rule — one input,
// one output, no state. Pipeline composes them; it does not know which
// rule any given strategy implements.
export interface Transformation {
  apply(input: string): string;
}

export class Pipeline {
  constructor(private readonly steps: readonly Transformation[]) {}

  run(input: string): string {
    return this.steps.reduce((current, step) => step.apply(current), input);
  }
}

const isLetter = (c: string): boolean => /^[A-Za-z]$/.test(c);
const isSeparator = (c: string): boolean => /\s/.test(c) || c === '-';

export class Capitalise implements Transformation {
  apply(input: string): string {
    if (input.length === 0) return input;
    const chars = [...input];
    let atWordStart = true;
    for (let i = 0; i < chars.length; i++) {
      const c = chars[i]!;
      const letter = isLetter(c);
      if (letter && atWordStart) chars[i] = c.toUpperCase();
      atWordStart = !letter;
    }
    return chars.join('');
  }
}

export class Reverse implements Transformation {
  apply(input: string): string {
    return [...input].reverse().join('');
  }
}

export class RemoveWhitespace implements Transformation {
  apply(input: string): string {
    return input.replace(/\s+/g, '');
  }
}

export class SnakeCase implements Transformation {
  apply(input: string): string {
    const lowered = input.toLowerCase();
    let out = '';
    let inSeparator = true; // suppress leading separators
    for (const c of lowered) {
      if (isSeparator(c)) {
        inSeparator = true;
      } else {
        if (inSeparator && out.length > 0) out += '_';
        out += c;
        inSeparator = false;
      }
    }
    return out;
  }
}

export class CamelCase implements Transformation {
  apply(input: string): string {
    let out = '';
    let tokenIndex = 0;
    let atTokenStart = true;
    for (const c of input) {
      if (isSeparator(c)) {
        if (!atTokenStart) {
          tokenIndex++;
          atTokenStart = true;
        }
      } else {
        const lower = c.toLowerCase();
        out += atTokenStart && tokenIndex > 0 ? lower.toUpperCase() : lower;
        atTokenStart = false;
      }
    }
    return out;
  }
}

export class Truncate implements Transformation {
  constructor(private readonly length: number) {}

  apply(input: string): string {
    if (input.length <= this.length) return input;
    return input.slice(0, this.length) + TRUNCATION_MARKER;
  }
}

export class Repeat implements Transformation {
  constructor(private readonly times: number) {}

  apply(input: string): string {
    if (this.times <= 0) return '';
    return Array(this.times).fill(input).join(' ');
  }
}

export class Replace implements Transformation {
  constructor(private readonly target: string, private readonly replacement: string) {}

  apply(input: string): string {
    return input.split(this.target).join(this.replacement);
  }
}
