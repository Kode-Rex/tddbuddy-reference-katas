export type Decision =
  | { readonly kind: 'allowed' }
  | { readonly kind: 'rejected'; readonly retryAfterMs: number };

export const allowed = (): Decision => ({ kind: 'allowed' });

export const rejected = (retryAfterMs: number): Decision => ({
  kind: 'rejected',
  retryAfterMs,
});
