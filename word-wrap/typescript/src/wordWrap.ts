export function wrap(text: string, width: number): string {
  const words = text.split(/\s+/).filter((w) => w.length > 0);
  if (words.length === 0) {
    return '';
  }

  const lines: string[] = [];
  let line = '';

  for (const original of words) {
    let word = original;
    while (word.length > width) {
      if (line.length > 0) {
        lines.push(line);
        line = '';
      }
      lines.push(word.slice(0, width));
      word = word.slice(width);
    }

    if (line.length === 0) {
      line = word;
    } else if (line.length + 1 + word.length <= width) {
      line = line + ' ' + word;
    } else {
      lines.push(line);
      line = word;
    }
  }

  if (line.length > 0) {
    lines.push(line);
  }

  return lines.join('\n');
}
