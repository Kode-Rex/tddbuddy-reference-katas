import type { MachineSelector } from '../src/MachineSelector.js';

export class FixedMachineSelector implements MachineSelector {
  constructor(private readonly machineNumber: number) {}

  selectAvailable(): number {
    return this.machineNumber;
  }
}
