import { describe, it, expect } from 'vitest';
import { Shortener } from '../src/shortener.js';

describe('Shortener', () => {
  it('shorten first url issues code 0', () => {
    const shortener = new Shortener();
    expect(shortener.shorten('https://example.com/alpha')).toBe('https://short.url/0');
  });

  it('second distinct url issues code 1', () => {
    const shortener = new Shortener();
    shortener.shorten('https://example.com/alpha');
    expect(shortener.shorten('https://example.com/beta')).toBe('https://short.url/1');
  });

  it('shortening a duplicate returns the existing short url', () => {
    const shortener = new Shortener();
    shortener.shorten('https://example.com/alpha');
    expect(shortener.shorten('https://example.com/alpha')).toBe('https://short.url/0');
  });

  it('duplicate does not advance the counter', () => {
    const shortener = new Shortener();
    shortener.shorten('https://example.com/alpha');
    shortener.shorten('https://example.com/alpha');
    expect(shortener.shorten('https://example.com/beta')).toBe('https://short.url/1');
  });

  it('eleventh distinct url issues code a', () => {
    const shortener = new Shortener();
    for (let i = 0; i < 10; i++) {
      shortener.shorten(`https://example.com/url-${i}`);
    }
    expect(shortener.shorten('https://example.com/url-10')).toBe('https://short.url/a');
  });

  it('translate by long url returns the short url', () => {
    const shortener = new Shortener();
    shortener.shorten('https://example.com/alpha');
    expect(shortener.translate('https://example.com/alpha')).toBe('https://short.url/0');
  });

  it('translate by short url returns the same short url', () => {
    const shortener = new Shortener();
    shortener.shorten('https://example.com/alpha');
    expect(shortener.translate('https://short.url/0')).toBe('https://short.url/0');
  });

  it('translate by short url increments visits', () => {
    const shortener = new Shortener();
    shortener.shorten('https://example.com/alpha');
    shortener.translate('https://short.url/0');
    shortener.translate('https://short.url/0');
    shortener.translate('https://short.url/0');
    expect(shortener.statistics('https://short.url/0').visits).toBe(3);
  });

  it('translate by long url does not increment visits', () => {
    const shortener = new Shortener();
    shortener.shorten('https://example.com/alpha');
    shortener.translate('https://example.com/alpha');
    shortener.translate('https://example.com/alpha');
    shortener.translate('https://example.com/alpha');
    expect(shortener.statistics('https://short.url/0').visits).toBe(0);
  });

  it('shorten does not count as a visit', () => {
    const shortener = new Shortener();
    shortener.shorten('https://example.com/alpha');
    expect(shortener.statistics('https://short.url/0').visits).toBe(0);
  });

  it('statistics by long url', () => {
    const shortener = new Shortener();
    shortener.shorten('https://example.com/alpha');
    shortener.translate('https://short.url/0');
    shortener.translate('https://short.url/0');
    expect(shortener.statistics('https://example.com/alpha')).toEqual({
      shortUrl: 'https://short.url/0',
      longUrl: 'https://example.com/alpha',
      visits: 2,
    });
  });

  it('statistics by short url', () => {
    const shortener = new Shortener();
    shortener.shorten('https://example.com/alpha');
    shortener.translate('https://short.url/0');
    shortener.translate('https://short.url/0');
    expect(shortener.statistics('https://short.url/0')).toEqual({
      shortUrl: 'https://short.url/0',
      longUrl: 'https://example.com/alpha',
      visits: 2,
    });
  });

  it('translate on unknown url raises', () => {
    const shortener = new Shortener();
    expect(() => shortener.translate('https://unknown.example/x'))
      .toThrow('Unknown URL: https://unknown.example/x');
  });

  it('statistics on unknown url raises', () => {
    const shortener = new Shortener();
    expect(() => shortener.statistics('https://unknown.example/x'))
      .toThrow('Unknown URL: https://unknown.example/x');
  });
});
