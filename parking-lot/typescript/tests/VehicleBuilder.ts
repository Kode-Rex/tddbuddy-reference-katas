import type { Vehicle } from '../src/Vehicle.js';
import type { VehicleType } from '../src/VehicleType.js';

export class VehicleBuilder {
  private type: VehicleType = 'car';
  private plate = 'CAR-001';

  asMotorcycle(): this { this.type = 'motorcycle'; this.plate = 'MC-001'; return this; }
  asCar(): this { this.type = 'car'; this.plate = 'CAR-001'; return this; }
  asBus(): this { this.type = 'bus'; this.plate = 'BUS-001'; return this; }
  withPlate(plate: string): this { this.plate = plate; return this; }

  build(): Vehicle {
    return { type: this.type, licensePlate: this.plate };
  }
}
