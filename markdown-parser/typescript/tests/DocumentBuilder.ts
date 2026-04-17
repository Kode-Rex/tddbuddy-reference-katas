export class DocumentBuilder {
  private lines: string[] = [];

  withLine(line: string): this { this.lines.push(line); return this; }
  withBlankLine(): this { this.lines.push(''); return this; }
  withHeading(level: number, text: string): this { this.lines.push(`${'#'.repeat(level)} ${text}`); return this; }
  withParagraph(text: string): this { this.lines.push(text); return this; }
  withListItem(text: string): this { this.lines.push(`- ${text}`); return this; }
  withBlockquote(text: string): this { this.lines.push(`> ${text}`); return this; }
  withCodeBlock(content: string): this {
    this.lines.push('```');
    this.lines.push(content);
    this.lines.push('```');
    return this;
  }

  build(): string { return this.lines.join('\n'); }
}
