import { describe, expect, it } from 'vitest';
import { parse } from '../src/MarkdownParser.js';
import { DocumentBuilder } from './DocumentBuilder.js';

describe('MarkdownParser', () => {
  // --- Empty and Plain Text ---

  it('empty input returns empty output', () => {
    expect(parse('')).toBe('');
  });

  it('plain text is wrapped in a paragraph', () => {
    expect(parse('Hello')).toBe('<p>Hello</p>');
  });

  it('multiple words in plain text are preserved', () => {
    expect(parse('Hello World')).toBe('<p>Hello World</p>');
  });

  // --- Headings ---

  it('single hash with space produces h1', () => {
    expect(parse('# Heading')).toBe('<h1>Heading</h1>');
  });

  it('double hash with space produces h2', () => {
    expect(parse('## Sub Heading')).toBe('<h2>Sub Heading</h2>');
  });

  it('six hashes with space produces h6', () => {
    expect(parse('###### Smallest')).toBe('<h6>Smallest</h6>');
  });

  it('hash without space is treated as a paragraph', () => {
    expect(parse('#No space')).toBe('<p>#No space</p>');
  });

  it('inline formatting inside headings is applied', () => {
    expect(parse('# **Bold** Heading')).toBe('<h1><strong>Bold</strong> Heading</h1>');
  });

  // --- Bold ---

  it('double asterisks produce strong', () => {
    expect(parse('**bold**')).toBe('<p><strong>bold</strong></p>');
  });

  it('bold text surrounded by plain text', () => {
    expect(parse('before **bold** after')).toBe('<p>before <strong>bold</strong> after</p>');
  });

  // --- Italic ---

  it('underscores produce emphasis', () => {
    expect(parse('_italic_')).toBe('<p><em>italic</em></p>');
  });

  it('italic text surrounded by plain text', () => {
    expect(parse('before _italic_ after')).toBe('<p>before <em>italic</em> after</p>');
  });

  // --- Nested Inline Formatting ---

  it('bold wrapping italic produces nested tags', () => {
    expect(parse('**_bold italic_**')).toBe('<p><strong><em>bold italic</em></strong></p>');
  });

  it('bold and italic in the same line', () => {
    expect(parse('**bold** and _italic_')).toBe('<p><strong>bold</strong> and <em>italic</em></p>');
  });

  // --- Links ---

  it('link syntax produces anchor tag', () => {
    expect(parse('[click](http://example.com)')).toBe('<p><a href="http://example.com">click</a></p>');
  });

  it('link surrounded by plain text', () => {
    expect(parse('see [here](http://example.com) now')).toBe(
      '<p>see <a href="http://example.com">here</a> now</p>',
    );
  });

  // --- Unordered Lists ---

  it('single list item produces ul with one li', () => {
    expect(parse('- item one')).toBe('<ul><li>item one</li></ul>');
  });

  it('multiple consecutive list items grouped into one ul', () => {
    const markdown = new DocumentBuilder()
      .withListItem('item one')
      .withListItem('item two')
      .withListItem('item three')
      .build();

    expect(parse(markdown)).toBe('<ul><li>item one</li><li>item two</li><li>item three</li></ul>');
  });

  it('inline formatting inside list items is applied', () => {
    expect(parse('- **bold** item')).toBe('<ul><li><strong>bold</strong> item</li></ul>');
  });

  // --- Multiple Paragraphs ---

  it('blank line separates two paragraphs', () => {
    const markdown = new DocumentBuilder()
      .withParagraph('first')
      .withBlankLine()
      .withParagraph('second')
      .build();

    expect(parse(markdown)).toBe('<p>first</p><p>second</p>');
  });

  // --- Inline Code ---

  it('backtick wrapped text produces code element', () => {
    expect(parse('`code`')).toBe('<p><code>code</code></p>');
  });

  it('inline code does not process nested formatting', () => {
    expect(parse('`**not bold**`')).toBe('<p><code>**not bold**</code></p>');
  });

  // --- Code Blocks ---

  it('fenced code block produces pre and code elements', () => {
    const markdown = new DocumentBuilder().withCodeBlock('let x = 1;').build();

    expect(parse(markdown)).toBe('<pre><code>let x = 1;</code></pre>');
  });

  it('code block preserves content verbatim', () => {
    const markdown = new DocumentBuilder().withCodeBlock('**not bold**').build();

    expect(parse(markdown)).toBe('<pre><code>**not bold**</code></pre>');
  });

  // --- Blockquotes ---

  it('line starting with angle bracket produces blockquote', () => {
    expect(parse('> quoted text')).toBe('<blockquote>quoted text</blockquote>');
  });

  // --- Escaped Characters ---

  it('escaped asterisks render as literal asterisks', () => {
    expect(parse('\\*not bold\\*')).toBe('<p>*not bold*</p>');
  });

  it('escaped underscores render as literal underscores', () => {
    expect(parse('\\_not italic\\_')).toBe('<p>_not italic_</p>');
  });
});
