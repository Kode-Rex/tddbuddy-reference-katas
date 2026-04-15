const DEFAULT_DELIMITER = /[,\n]/;

export interface ParsedInput {
  delimiter: RegExp;
  body: string;
}

export function parse(input: string): ParsedInput {
  if (!input.startsWith('//')) {
    return { delimiter: DEFAULT_DELIMITER, body: input };
  }

  const headerEnd = input.indexOf('\n');
  const header = input.substring(2, headerEnd);
  const body = input.substring(headerEnd + 1);
  return { delimiter: new RegExp(header), body };
}
