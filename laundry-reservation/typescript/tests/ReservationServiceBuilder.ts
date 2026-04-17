import { MachineApi } from '../src/MachineApi.js';
import { ReservationService } from '../src/ReservationService.js';
import { FixedMachineSelector } from './FixedMachineSelector.js';
import { FixedPinGenerator } from './FixedPinGenerator.js';
import { InMemoryReservationRepository } from './InMemoryReservationRepository.js';
import { RecordingEmailNotifier } from './RecordingEmailNotifier.js';
import { RecordingMachineDevice } from './RecordingMachineDevice.js';
import { RecordingSmsNotifier } from './RecordingSmsNotifier.js';

export class ReservationServiceBuilder {
  private machineNumber = 7;
  private pins: number[] = [12345];
  readonly device = new RecordingMachineDevice();
  readonly emailNotifier = new RecordingEmailNotifier();
  readonly smsNotifier = new RecordingSmsNotifier();
  readonly repository = new InMemoryReservationRepository();

  withMachineNumber(n: number): this { this.machineNumber = n; return this; }
  withPins(...pins: number[]): this { this.pins = pins; return this; }
  withDeviceRejectingLock(): this { this.device.shouldAcceptLock = false; return this; }

  build(): { service: ReservationService; machineApi: MachineApi } {
    const machineApi = new MachineApi();
    machineApi.registerDevice(this.machineNumber, this.device);

    const service = new ReservationService(
      this.repository,
      this.emailNotifier,
      this.smsNotifier,
      machineApi,
      new FixedPinGenerator(...this.pins),
      new FixedMachineSelector(this.machineNumber),
    );

    return { service, machineApi };
  }
}
