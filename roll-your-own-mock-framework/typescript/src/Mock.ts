import { CallRecord } from './CallRecord.js';
import { VerificationError } from './VerificationError.js';

interface StubEntry {
  args: unknown[];
  returnValue: unknown;
}

interface MethodVerification {
  wasCalled(): void;
  wasCalledWith(...args: unknown[]): void;
  wasCalledTimes(n: number): void;
}

interface StubConfiguration {
  thenReturn(value: unknown): void;
}

// eslint-disable-next-line @typescript-eslint/no-explicit-any
type MockProxy = Record<string, (...args: any[]) => unknown>;

class MockState {
  readonly calls: CallRecord[] = [];
  private readonly stubs: Map<string, StubEntry[]> = new Map();

  recordCall(methodName: string, args: unknown[]): void {
    this.calls.push({ methodName, args });
  }

  addStub(methodName: string, args: unknown[], returnValue: unknown): void {
    if (!this.stubs.has(methodName)) {
      this.stubs.set(methodName, []);
    }
    this.stubs.get(methodName)!.push({ args, returnValue });
  }

  findStub(methodName: string, args: unknown[]): unknown {
    const entries = this.stubs.get(methodName);
    if (!entries) return undefined;
    for (const entry of entries) {
      if (argsMatch(entry.args, args)) {
        return entry.returnValue;
      }
    }
    return undefined;
  }
}

function argsMatch(expected: unknown[], actual: unknown[]): boolean {
  if (expected.length !== actual.length) return false;
  return expected.every((val, i) => val === actual[i]);
}

function formatArgs(args: unknown[]): string {
  return args.map(a => String(a)).join(', ');
}

const stateMap = new WeakMap<MockProxy, MockState>();

export function createMock(): MockProxy {
  const state = new MockState();

  const proxy = new Proxy({} as MockProxy, {
    get(_target, prop: string) {
      return (...args: unknown[]): unknown => {
        state.recordCall(prop, args);
        return state.findStub(prop, args);
      };
    },
  });

  stateMap.set(proxy, state);
  return proxy;
}

export function when(mock: MockProxy): MockProxy {
  const state = stateMap.get(mock);
  if (!state) throw new Error('not a mock');

  return new Proxy({} as MockProxy, {
    get(_target, prop: string) {
      return (...args: unknown[]): StubConfiguration => ({
        thenReturn(value: unknown): void {
          state.addStub(prop, args, value);
        },
      });
    },
  });
}

export function verify(mock: MockProxy): Record<string, MethodVerification> {
  const state = stateMap.get(mock);
  if (!state) throw new Error('not a mock');

  return new Proxy({} as Record<string, MethodVerification>, {
    get(_target, prop: string): MethodVerification {
      return {
        wasCalled(): void {
          const called = state.calls.some(c => c.methodName === prop);
          if (!called) {
            throw new VerificationError(`expected ${prop} to be called but was never called`);
          }
        },

        wasCalledWith(...expectedArgs: unknown[]): void {
          const methodCalls = state.calls.filter(c => c.methodName === prop);
          if (methodCalls.length === 0) {
            throw new VerificationError(
              `expected ${prop}(${formatArgs(expectedArgs)}) to be called but was never called`,
            );
          }
          const match = methodCalls.some(c => argsMatch(expectedArgs, c.args));
          if (!match) {
            throw new VerificationError(
              `expected ${prop}(${formatArgs(expectedArgs)}) to be called but was called with (${formatArgs(methodCalls[0]!.args)})`,
            );
          }
        },

        wasCalledTimes(n: number): void {
          const actualCount = state.calls.filter(c => c.methodName === prop).length;
          if (actualCount !== n) {
            throw new VerificationError(
              `expected ${prop} to be called ${n} times but was called ${actualCount} times`,
            );
          }
        },
      };
    },
  });
}
