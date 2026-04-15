export interface UrlStatistics {
  shortUrl: string;
  longUrl: string;
  visits: number;
}

const SHORT_URL_BASE = 'https://short.url/';

export class Shortener {
  private readonly longToCode = new Map<string, string>();
  private readonly codeToLong = new Map<string, string>();
  private readonly visits = new Map<string, number>();
  private nextCounter = 0;

  shorten(longUrl: string): string {
    const existing = this.longToCode.get(longUrl);
    if (existing !== undefined) {
      return SHORT_URL_BASE + existing;
    }

    const code = toBase36(this.nextCounter++);
    this.longToCode.set(longUrl, code);
    this.codeToLong.set(code, longUrl);
    this.visits.set(code, 0);
    return SHORT_URL_BASE + code;
  }

  translate(url: string): string {
    const shortCode = this.resolveByShortUrl(url);
    if (shortCode !== undefined) {
      this.visits.set(shortCode, (this.visits.get(shortCode) ?? 0) + 1);
      return SHORT_URL_BASE + shortCode;
    }

    const codeFromLong = this.longToCode.get(url);
    if (codeFromLong !== undefined) {
      return SHORT_URL_BASE + codeFromLong;
    }

    throw new Error(`Unknown URL: ${url}`);
  }

  statistics(url: string): UrlStatistics {
    const shortCode = this.resolveByShortUrl(url);
    if (shortCode !== undefined) {
      return this.buildStatistics(shortCode);
    }

    const codeFromLong = this.longToCode.get(url);
    if (codeFromLong !== undefined) {
      return this.buildStatistics(codeFromLong);
    }

    throw new Error(`Unknown URL: ${url}`);
  }

  private resolveByShortUrl(url: string): string | undefined {
    if (!url.startsWith(SHORT_URL_BASE)) return undefined;
    const candidate = url.slice(SHORT_URL_BASE.length);
    return this.codeToLong.has(candidate) ? candidate : undefined;
  }

  private buildStatistics(code: string): UrlStatistics {
    return {
      shortUrl: SHORT_URL_BASE + code,
      longUrl: this.codeToLong.get(code) as string,
      visits: this.visits.get(code) ?? 0,
    };
  }
}

function toBase36(value: number): string {
  return value.toString(36);
}
