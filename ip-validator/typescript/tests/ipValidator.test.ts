import { describe, it, expect } from 'vitest';
import { isValid } from '../src/ipValidator.js';

describe('isValid', () => {
  it('accepts 1.1.1.1', () => {
    expect(isValid('1.1.1.1')).toBe(true);
  });

  it('accepts 192.168.1.1', () => {
    expect(isValid('192.168.1.1')).toBe(true);
  });

  it('accepts 10.0.0.1', () => {
    expect(isValid('10.0.0.1')).toBe(true);
  });

  it('accepts 127.0.0.1', () => {
    expect(isValid('127.0.0.1')).toBe(true);
  });

  it('rejects 0.0.0.0 because last octet is zero', () => {
    expect(isValid('0.0.0.0')).toBe(false);
  });

  it('rejects 255.255.255.255 because last octet is broadcast', () => {
    expect(isValid('255.255.255.255')).toBe(false);
  });

  it('rejects 192.168.1.0 because last octet is zero', () => {
    expect(isValid('192.168.1.0')).toBe(false);
  });

  it('rejects 192.168.1.255 because last octet is broadcast', () => {
    expect(isValid('192.168.1.255')).toBe(false);
  });

  it('rejects 10.0.1 because it has only three octets', () => {
    expect(isValid('10.0.1')).toBe(false);
  });

  it('rejects 1.2.3.4.5 because it has five octets', () => {
    expect(isValid('1.2.3.4.5')).toBe(false);
  });

  it('rejects 192.168.01.1 because an octet has a leading zero', () => {
    expect(isValid('192.168.01.1')).toBe(false);
  });

  it('rejects 192.168.1.00 because an octet has leading zeros', () => {
    expect(isValid('192.168.1.00')).toBe(false);
  });

  it('rejects 256.1.1.1 because first octet exceeds 255', () => {
    expect(isValid('256.1.1.1')).toBe(false);
  });

  it('rejects 1.1.1.999 because last octet exceeds 255', () => {
    expect(isValid('1.1.1.999')).toBe(false);
  });

  it('rejects an address with a negative octet', () => {
    expect(isValid('1.1.1.-1')).toBe(false);
  });

  it('rejects an address with a non-digit character', () => {
    expect(isValid('1.1.1.a')).toBe(false);
  });

  it('rejects an address with an empty octet', () => {
    expect(isValid('1.1..1')).toBe(false);
  });

  it('rejects the empty string', () => {
    expect(isValid('')).toBe(false);
  });
});
