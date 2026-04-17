import type { Vehicle } from './Vehicle.js';
import type { SpotType } from './SpotType.js';

export interface Ticket {
  readonly vehicle: Vehicle;
  readonly spotType: SpotType;
  readonly entryTime: Date;
}
