const DEFAULT_DELIMITER = /[,\n]/;

export interface ParsedInput {
  delimiter: RegExp;
  body: string;
}

function escapeForRegex(literal: string): string {
  return literal.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
}

export function parse(input: string): ParsedInput {
  if (!input.startsWith('//')) {
    return { delimiter: DEFAULT_DELIMITER, body: input };
  }

  const headerEnd = input.indexOf('\n');
  const header = input.substring(2, headerEnd);
  const body = input.substring(headerEnd + 1);

  let literals: string[];
  if (header.startsWith('[')) {
    literals = Array.from(header.matchAll(/\[([^\]]+)\]/g), (m) => m[1]!);
  } else {
    literals = [header];
  }

  const pattern = literals.map(escapeForRegex).join('|');
  return { delimiter: new RegExp(pattern), body };
}
