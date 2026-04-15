export class LimiterRuleInvalidError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'LimiterRuleInvalidError';
  }
}
