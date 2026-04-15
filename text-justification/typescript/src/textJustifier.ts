export function justify(text: string, width: number): readonly string[] {
  const words = text.split(/\s+/).filter((w) => w.length > 0);
  if (words.length === 0) {
    return [];
  }

  const lines: string[] = [];
  let lineWords: string[] = [];
  let lineContentLength = 0;

  for (const word of words) {
    const projected = lineContentLength + lineWords.length + word.length;
    if (lineWords.length > 0 && projected > width) {
      lines.push(justifyLine(lineWords, lineContentLength, width));
      lineWords = [];
      lineContentLength = 0;
    }
    lineWords.push(word);
    lineContentLength += word.length;
  }

  if (lineWords.length > 0) {
    lines.push(lineWords.join(' '));
  }

  return lines;
}

function justifyLine(lineWords: string[], contentLength: number, width: number): string {
  if (lineWords.length === 1) {
    const only = lineWords[0]!;
    return only.length >= width ? only : only + ' '.repeat(width - only.length);
  }

  const gaps = lineWords.length - 1;
  const padding = width - contentLength;
  const baseSpaces = Math.floor(padding / gaps);
  const extras = padding % gaps;

  let out = '';
  for (let i = 0; i < lineWords.length; i++) {
    out += lineWords[i];
    if (i < gaps) {
      const spaces = baseSpaces + (i < extras ? 1 : 0);
      out += ' '.repeat(spaces);
    }
  }
  return out;
}
