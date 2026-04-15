// Identical byte-for-byte across C#, TypeScript, and Python.
// The failure message strings are the spec (see ../SCENARIOS.md).
export const RuleNames = {
  minimumLength: 'minimum length',
  requiresDigit: 'requires digit',
  requiresSymbol: 'requires symbol',
  requiresUppercase: 'requires uppercase',
  requiresLowercase: 'requires lowercase',
} as const;

export const DEFAULT_MIN_LENGTH = 8;

export interface ValidationResult {
  readonly ok: boolean;
  readonly failures: readonly string[];
}

export interface Policy {
  readonly minLength: number;
  readonly requiresDigit: boolean;
  readonly requiresSymbol: boolean;
  readonly requiresUpper: boolean;
  readonly requiresLower: boolean;
  validate(password: string): ValidationResult;
}

export function createPolicy(spec: {
  minLength: number;
  requiresDigit: boolean;
  requiresSymbol: boolean;
  requiresUpper: boolean;
  requiresLower: boolean;
}): Policy {
  return {
    ...spec,
    validate(password: string): ValidationResult {
      const failures: string[] = [];
      if (password.length < spec.minLength) failures.push(RuleNames.minimumLength);
      if (spec.requiresDigit && !/[0-9]/.test(password)) failures.push(RuleNames.requiresDigit);
      if (spec.requiresSymbol && !/[^A-Za-z0-9]/.test(password)) failures.push(RuleNames.requiresSymbol);
      if (spec.requiresUpper && !/[A-Z]/.test(password)) failures.push(RuleNames.requiresUppercase);
      if (spec.requiresLower && !/[a-z]/.test(password)) failures.push(RuleNames.requiresLowercase);
      return { ok: failures.length === 0, failures };
    },
  };
}
