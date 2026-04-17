import type { MachineDevice } from './MachineDevice.js';

export class MachineApi {
  private readonly devices = new Map<number, MachineDevice>();
  private readonly reservationToMachine = new Map<string, number>();

  registerDevice(machineNumber: number, device: MachineDevice): void {
    this.devices.set(machineNumber, device);
  }

  lock(reservationId: string, machineNumber: number, reservationDateTime: Date, pin: number): boolean {
    const device = this.devices.get(machineNumber);
    if (!device) return false;

    if (this.reservationToMachine.has(reservationId)) {
      device.lock(reservationId, reservationDateTime, pin);
      return true;
    }

    const locked = device.lock(reservationId, reservationDateTime, pin);
    if (locked) {
      this.reservationToMachine.set(reservationId, machineNumber);
    }
    return locked;
  }

  unlock(machineNumber: number, reservationId: string): void {
    const device = this.devices.get(machineNumber);
    if (device) {
      device.unlock(reservationId);
      this.reservationToMachine.delete(reservationId);
    }
  }
}
