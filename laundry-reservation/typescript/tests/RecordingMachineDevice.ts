import type { MachineDevice } from '../src/MachineDevice.js';

export interface LockCall {
  reservationId: string;
  reservationDateTime: Date;
  pin: number;
}

export class RecordingMachineDevice implements MachineDevice {
  readonly lockCalls: LockCall[] = [];
  readonly unlockCalls: string[] = [];
  shouldAcceptLock = true;

  lock(reservationId: string, reservationDateTime: Date, pin: number): boolean {
    this.lockCalls.push({ reservationId, reservationDateTime, pin });
    return this.shouldAcceptLock;
  }

  unlock(reservationId: string): void {
    this.unlockCalls.push(reservationId);
  }
}
