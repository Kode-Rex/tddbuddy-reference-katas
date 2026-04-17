import type { VehicleType } from './VehicleType.js';

export interface Vehicle {
  readonly type: VehicleType;
  readonly licensePlate: string;
}
