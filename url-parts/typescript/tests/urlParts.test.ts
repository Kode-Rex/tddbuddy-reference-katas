import { describe, it, expect } from 'vitest';
import { parse } from '../src/urlParts.js';

describe('parse', () => {
  it('parses http root with www subdomain and default port', () => {
    expect(parse('http://www.tddbuddy.com')).toEqual({
      protocol: 'http', subdomain: 'www', domain: 'tddbuddy.com',
      port: 80, path: '', parameters: '', anchor: '',
    });
  });

  it('parses http with single-segment subdomain and path', () => {
    expect(parse('http://foo.bar.com/foobar.html')).toEqual({
      protocol: 'http', subdomain: 'foo', domain: 'bar.com',
      port: 80, path: 'foobar.html', parameters: '', anchor: '',
    });
  });

  it('parses https with explicit port and multi-segment path', () => {
    expect(parse('https://www.foobar.com:8080/download/install.exe')).toEqual({
      protocol: 'https', subdomain: 'www', domain: 'foobar.com',
      port: 8080, path: 'download/install.exe', parameters: '', anchor: '',
    });
  });

  it('parses ftp with no subdomain and explicit port', () => {
    expect(parse('ftp://foo.com:9000/files')).toEqual({
      protocol: 'ftp', subdomain: '', domain: 'foo.com',
      port: 9000, path: 'files', parameters: '', anchor: '',
    });
  });

  it('parses https localhost with path and anchor', () => {
    expect(parse('https://localhost/index.html#footer')).toEqual({
      protocol: 'https', subdomain: '', domain: 'localhost',
      port: 443, path: 'index.html', parameters: '', anchor: 'footer',
    });
  });

  it('parses sftp with default port 22', () => {
    expect(parse('sftp://user.example.org/path')).toEqual({
      protocol: 'sftp', subdomain: 'user', domain: 'example.org',
      port: 22, path: 'path', parameters: '', anchor: '',
    });
  });

  it('parses http with query parameters', () => {
    expect(parse('http://api.example.com/search?q=tdd&page=2')).toEqual({
      protocol: 'http', subdomain: 'api', domain: 'example.com',
      port: 80, path: 'search', parameters: 'q=tdd&page=2', anchor: '',
    });
  });

  it('parses https with query parameters and anchor', () => {
    expect(parse('https://www.site.net/page?id=42#section')).toEqual({
      protocol: 'https', subdomain: 'www', domain: 'site.net',
      port: 443, path: 'page', parameters: 'id=42', anchor: 'section',
    });
  });

  it('parses http localhost with explicit port and no path', () => {
    expect(parse('http://localhost:3000')).toEqual({
      protocol: 'http', subdomain: '', domain: 'localhost',
      port: 3000, path: '', parameters: '', anchor: '',
    });
  });

  it('parses https with explicit port and no path', () => {
    expect(parse('https://www.example.gov:8443')).toEqual({
      protocol: 'https', subdomain: 'www', domain: 'example.gov',
      port: 8443, path: '', parameters: '', anchor: '',
    });
  });
});
