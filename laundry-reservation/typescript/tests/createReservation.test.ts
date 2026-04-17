import { Customer } from '../src/Customer.js';
import { DuplicateReservationError } from '../src/Exceptions.js';
import { ReservationServiceBuilder } from './ReservationServiceBuilder.js';

const slot = new Date(Date.UTC(2026, 3, 15, 10, 0, 0));
const customer = new Customer('alice@example.com', '+27821234567');

describe('Create Reservation', () => {
  it('Creating a reservation assigns a unique reservation ID', () => {
    const builder = new ReservationServiceBuilder();
    const { service } = builder.build();

    const reservation = service.createReservation(slot, customer);

    expect(reservation.id).toBeTruthy();
  });

  it('Creating a reservation assigns a machine number from the selector', () => {
    const builder = new ReservationServiceBuilder().withMachineNumber(14);
    const { service } = builder.build();

    const reservation = service.createReservation(slot, customer);

    expect(reservation.machineNumber).toBe(14);
  });

  it('Creating a reservation assigns a five-digit PIN from the generator', () => {
    const builder = new ReservationServiceBuilder().withPins(98765);
    const { service } = builder.build();

    const reservation = service.createReservation(slot, customer);

    expect(reservation.pin).toBe(98765);
  });

  it('Creating a reservation sends a confirmation email with machine number, reservation ID, and PIN', () => {
    const builder = new ReservationServiceBuilder()
      .withMachineNumber(7)
      .withPins(12345);
    const { service } = builder.build();

    const reservation = service.createReservation(slot, customer);

    expect(builder.emailNotifier.sent).toHaveLength(1);
    const email = builder.emailNotifier.sent[0]!;
    expect(email.to).toBe('alice@example.com');
    expect(email.body).toContain('Machine 7');
    expect(email.body).toContain(reservation.id);
    expect(email.body).toContain('12345');
  });

  it('Creating a reservation saves the reservation to the repository', () => {
    const builder = new ReservationServiceBuilder();
    const { service } = builder.build();

    const reservation = service.createReservation(slot, customer);

    expect(builder.repository.all).toHaveLength(1);
    expect(builder.repository.all[0]!.id).toBe(reservation.id);
  });

  it('Creating a reservation locks the machine via the Machine API', () => {
    const builder = new ReservationServiceBuilder()
      .withMachineNumber(7)
      .withPins(12345);
    const { service } = builder.build();

    const reservation = service.createReservation(slot, customer);

    expect(builder.device.lockCalls).toHaveLength(1);
    const lockCall = builder.device.lockCalls[0]!;
    expect(lockCall.reservationId).toBe(reservation.id);
    expect(lockCall.reservationDateTime).toEqual(slot);
    expect(lockCall.pin).toBe(12345);
  });

  it('Creating a second reservation for the same customer is rejected', () => {
    const builder = new ReservationServiceBuilder().withPins(12345, 67890);
    const { service } = builder.build();
    service.createReservation(slot, customer);

    expect(() => service.createReservation(new Date(slot.getTime() + 7200000), customer))
      .toThrow(DuplicateReservationError);
    expect(() => service.createReservation(new Date(slot.getTime() + 7200000), customer))
      .toThrow("Customer 'alice@example.com' already has an active reservation.");
  });
});
