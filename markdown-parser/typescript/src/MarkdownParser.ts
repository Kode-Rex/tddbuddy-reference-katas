const EscapedAsterisk = '\uFFF0';
const EscapedUnderscore = '\uFFF1';
const EscapedBacktick = '\uFFF2';
const CodeSpanMarker = '\uFFF3';

function isHeading(line: string): boolean {
  return /^#{1,6} .+$/.test(line);
}

function parseHeading(line: string): string | null {
  const match = line.match(/^(#{1,6}) (.+)$/);
  if (!match) return null;
  const level = match[1]!.length;
  const content = applyInlineFormatting(match[2]!);
  return `<h${level}>${content}</h${level}>`;
}

function applyInlineFormatting(text: string): string {
  // Escape sequences first
  text = text.replaceAll('\\*', EscapedAsterisk);
  text = text.replaceAll('\\_', EscapedUnderscore);
  text = text.replaceAll('\\`', EscapedBacktick);

  // Extract inline code spans before other formatting
  const codeSpans: string[] = [];
  text = text.replace(/`([^`]+)`/g, (_match, content: string) => {
    codeSpans.push(content);
    return `${CodeSpanMarker}${codeSpans.length - 1}${CodeSpanMarker}`;
  });

  // Bold
  text = text.replace(/\*\*(.+?)\*\*/g, '<strong>$1</strong>');

  // Italic
  text = text.replace(/_(.+?)_/g, '<em>$1</em>');

  // Links
  text = text.replace(/\[([^\]]+)\]\(([^)]+)\)/g, '<a href="$2">$1</a>');

  // Restore inline code spans
  text = text.replace(new RegExp(`${CodeSpanMarker}(\\d+)${CodeSpanMarker}`, 'g'), (_match, idx: string) =>
    `<code>${codeSpans[parseInt(idx)]}</code>`,
  );

  // Restore escaped characters
  text = text.replaceAll(EscapedAsterisk, '*');
  text = text.replaceAll(EscapedUnderscore, '_');
  text = text.replaceAll(EscapedBacktick, '`');

  return text;
}

export function parse(markdown: string): string {
  if (markdown === '') return '';

  const lines = markdown.split('\n');
  const blocks: string[] = [];
  let i = 0;

  while (i < lines.length) {
    const line = lines[i]!;

    // Code block (fenced)
    if (line.trimStart().startsWith('```')) {
      i++;
      const codeLines: string[] = [];
      while (i < lines.length && !lines[i]!.trimStart().startsWith('```')) {
        codeLines.push(lines[i]!);
        i++;
      }
      if (i < lines.length) i++; // skip closing fence
      blocks.push(`<pre><code>${codeLines.join('\n')}</code></pre>`);
      continue;
    }

    // Blank line
    if (line.trim() === '') {
      i++;
      continue;
    }

    // Heading
    const heading = parseHeading(line);
    if (heading) {
      blocks.push(heading);
      i++;
      continue;
    }

    // Unordered list
    if (line.startsWith('- ')) {
      const items: string[] = [];
      while (i < lines.length && lines[i]!.startsWith('- ')) {
        items.push(`<li>${applyInlineFormatting(lines[i]!.slice(2))}</li>`);
        i++;
      }
      blocks.push(`<ul>${items.join('')}</ul>`);
      continue;
    }

    // Blockquote
    if (line.startsWith('> ')) {
      const content = applyInlineFormatting(line.slice(2));
      blocks.push(`<blockquote>${content}</blockquote>`);
      i++;
      continue;
    }

    // Paragraph
    {
      const paragraphLines: string[] = [];
      while (
        i < lines.length &&
        lines[i]!.trim() !== '' &&
        !lines[i]!.startsWith('- ') &&
        !lines[i]!.startsWith('> ') &&
        !lines[i]!.trimStart().startsWith('```') &&
        !isHeading(lines[i]!)
      ) {
        paragraphLines.push(lines[i]!);
        i++;
      }
      const text = paragraphLines.join(' ');
      blocks.push(`<p>${applyInlineFormatting(text)}</p>`);
    }
  }

  return blocks.join('');
}
