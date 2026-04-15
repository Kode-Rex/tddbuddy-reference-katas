export interface UrlParts {
  protocol: string;
  subdomain: string;
  domain: string;
  port: number;
  path: string;
  parameters: string;
  anchor: string;
}

const TOP_LEVEL_DOMAINS = ['.com', '.net', '.org', '.int', '.edu', '.gov', '.mil'];

const DEFAULT_PORTS: Record<string, number> = {
  http: 80,
  https: 443,
  ftp: 21,
  sftp: 22,
};

export function parse(url: string): UrlParts {
  const schemeSeparator = url.indexOf('://');
  const protocol = url.slice(0, schemeSeparator);
  let rest = url.slice(schemeSeparator + 3);

  let anchor = '';
  const hashIndex = rest.indexOf('#');
  if (hashIndex >= 0) {
    anchor = rest.slice(hashIndex + 1);
    rest = rest.slice(0, hashIndex);
  }

  let parameters = '';
  const queryIndex = rest.indexOf('?');
  if (queryIndex >= 0) {
    parameters = rest.slice(queryIndex + 1);
    rest = rest.slice(0, queryIndex);
  }

  let path = '';
  const slashIndex = rest.indexOf('/');
  if (slashIndex >= 0) {
    path = rest.slice(slashIndex + 1);
    rest = rest.slice(0, slashIndex);
  }

  const authority = rest;
  const colonIndex = authority.indexOf(':');
  let hostPart: string;
  let port: number;
  if (colonIndex >= 0) {
    hostPart = authority.slice(0, colonIndex);
    port = Number(authority.slice(colonIndex + 1));
  } else {
    hostPart = authority;
    const defaultPort = DEFAULT_PORTS[protocol];
    if (defaultPort === undefined) {
      throw new Error(`Unknown protocol: ${protocol}`);
    }
    port = defaultPort;
  }

  const { subdomain, domain } = splitHost(hostPart);

  return { protocol, subdomain, domain, port, path, parameters, anchor };
}

function splitHost(hostPart: string): { subdomain: string; domain: string } {
  if (hostPart === 'localhost') return { subdomain: '', domain: 'localhost' };

  for (const tld of TOP_LEVEL_DOMAINS) {
    if (!hostPart.endsWith(tld)) continue;

    const beforeTld = hostPart.slice(0, hostPart.length - tld.length);
    const lastDot = beforeTld.lastIndexOf('.');
    if (lastDot < 0) {
      return { subdomain: '', domain: beforeTld + tld };
    }
    const subdomain = beforeTld.slice(0, lastDot);
    const hostname = beforeTld.slice(lastDot + 1);
    return { subdomain, domain: hostname + tld };
  }

  throw new Error(`Unrecognized host: ${hostPart}`);
}
