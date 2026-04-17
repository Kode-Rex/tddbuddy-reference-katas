import { randomUUID } from 'node:crypto';
import type { Customer } from './Customer.js';
import type { EmailNotifier } from './EmailNotifier.js';
import { DuplicateReservationError } from './Exceptions.js';
import type { MachineApi } from './MachineApi.js';
import type { MachineSelector } from './MachineSelector.js';
import type { PinGenerator } from './PinGenerator.js';
import { Reservation } from './Reservation.js';
import type { ReservationRepository } from './ReservationRepository.js';
import type { SmsNotifier } from './SmsNotifier.js';

const MAX_FAILED_ATTEMPTS = 5;

function formatPin(pin: number): string {
  return String(pin).padStart(5, '0');
}

export class ReservationService {
  private readonly failureCounts = new Map<number, number>();

  constructor(
    private readonly repository: ReservationRepository,
    private readonly emailNotifier: EmailNotifier,
    private readonly smsNotifier: SmsNotifier,
    private readonly machineApi: MachineApi,
    private readonly pinGenerator: PinGenerator,
    private readonly machineSelector: MachineSelector,
  ) {}

  createReservation(slot: Date, customer: Customer): Reservation {
    const existing = this.repository.findActiveByCustomerEmail(customer.email);
    if (existing) {
      throw new DuplicateReservationError(
        `Customer '${customer.email}' already has an active reservation.`,
      );
    }

    const id = randomUUID();
    const machineNumber = this.machineSelector.selectAvailable();
    const pin = this.pinGenerator.generate();
    const reservation = new Reservation(id, slot, machineNumber, customer, pin);

    this.repository.save(reservation);

    this.emailNotifier.send(
      customer.email,
      'Wunda Wash Reservation Confirmation',
      `Reservation ${id}: Machine ${machineNumber}, PIN ${formatPin(pin)}`,
    );

    this.machineApi.lock(id, machineNumber, slot, pin);

    return reservation;
  }

  claimReservation(machineNumber: number, pin: number): boolean {
    const reservation = this.repository.findActiveByMachineNumber(machineNumber);
    if (!reservation) return false;

    if (reservation.pin === pin) {
      reservation.markUsed();
      this.repository.save(reservation);
      this.machineApi.unlock(machineNumber, reservation.id);
      this.failureCounts.delete(machineNumber);
      return true;
    }

    let failures = (this.failureCounts.get(machineNumber) ?? 0) + 1;
    this.failureCounts.set(machineNumber, failures);

    if (failures >= MAX_FAILED_ATTEMPTS) {
      const newPin = this.pinGenerator.generate();
      reservation.updatePin(newPin);
      this.repository.save(reservation);
      this.smsNotifier.send(
        reservation.customer.cellPhone,
        `Your new Wunda Wash PIN is ${formatPin(newPin)}.`,
      );
      this.machineApi.lock(
        reservation.id,
        machineNumber,
        reservation.slot,
        newPin,
      );
      this.failureCounts.set(machineNumber, 0);
    }

    return false;
  }
}
