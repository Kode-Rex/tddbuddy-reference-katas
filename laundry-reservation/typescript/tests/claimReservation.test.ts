import { Customer } from '../src/Customer.js';
import { ReservationServiceBuilder } from './ReservationServiceBuilder.js';

const slot = new Date(Date.UTC(2026, 3, 15, 10, 0, 0));
const customer = new Customer('alice@example.com', '+27821234567');

describe('Claim Reservation', () => {
  it('Claiming with the correct PIN marks the reservation as used', () => {
    const builder = new ReservationServiceBuilder().withPins(12345);
    const { service } = builder.build();
    service.createReservation(slot, customer);

    service.claimReservation(7, 12345);

    expect(builder.repository.all[0]!.isUsed).toBe(true);
  });

  it('Claiming with the correct PIN unlocks the machine', () => {
    const builder = new ReservationServiceBuilder().withPins(12345);
    const { service } = builder.build();
    service.createReservation(slot, customer);

    service.claimReservation(7, 12345);

    expect(builder.device.unlockCalls).toHaveLength(1);
  });

  it('Claiming with an incorrect PIN does not unlock the machine', () => {
    const builder = new ReservationServiceBuilder().withPins(12345);
    const { service } = builder.build();
    service.createReservation(slot, customer);

    service.claimReservation(7, 99999);

    expect(builder.device.unlockCalls).toHaveLength(0);
  });

  it('Claiming with an incorrect PIN does not mark the reservation as used', () => {
    const builder = new ReservationServiceBuilder().withPins(12345);
    const { service } = builder.build();
    service.createReservation(slot, customer);

    service.claimReservation(7, 99999);

    expect(builder.repository.all[0]!.isUsed).toBe(false);
  });

  it('Five consecutive incorrect PINs sends an SMS with a new PIN', () => {
    const builder = new ReservationServiceBuilder().withPins(12345, 67890);
    const { service } = builder.build();
    service.createReservation(slot, customer);

    for (let i = 0; i < 5; i++) service.claimReservation(7, 99999);

    expect(builder.smsNotifier.sent).toHaveLength(1);
    const sms = builder.smsNotifier.sent[0]!;
    expect(sms.to).toBe('+27821234567');
    expect(sms.message).toBe('Your new Wunda Wash PIN is 67890.');
  });

  it('Five consecutive incorrect PINs updates the reservation with the new PIN', () => {
    const builder = new ReservationServiceBuilder().withPins(12345, 67890);
    const { service } = builder.build();
    service.createReservation(slot, customer);

    for (let i = 0; i < 5; i++) service.claimReservation(7, 99999);

    expect(builder.repository.all[0]!.pin).toBe(67890);
  });

  it('Five consecutive incorrect PINs re-locks the machine with the new PIN', () => {
    const builder = new ReservationServiceBuilder().withPins(12345, 67890);
    const { service } = builder.build();
    service.createReservation(slot, customer);

    for (let i = 0; i < 5; i++) service.claimReservation(7, 99999);

    expect(builder.device.lockCalls).toHaveLength(2);
    const reLock = builder.device.lockCalls[1]!;
    expect(reLock.pin).toBe(67890);
  });

  it('A successful claim resets the failure counter', () => {
    const builder = new ReservationServiceBuilder().withPins(12345);
    const { service } = builder.build();
    service.createReservation(slot, customer);

    for (let i = 0; i < 4; i++) service.claimReservation(7, 99999);
    service.claimReservation(7, 12345);

    expect(builder.smsNotifier.sent).toHaveLength(0);
  });

  it('The failure counter resets after a new PIN is issued allowing five more attempts', () => {
    const builder = new ReservationServiceBuilder().withPins(12345, 67890, 11111);
    const { service } = builder.build();
    service.createReservation(slot, customer);

    // First round: 5 bad PINs triggers new PIN (67890)
    for (let i = 0; i < 5; i++) service.claimReservation(7, 99999);
    expect(builder.smsNotifier.sent).toHaveLength(1);

    // Second round: 4 bad PINs should not trigger another SMS
    for (let i = 0; i < 4; i++) service.claimReservation(7, 99999);
    expect(builder.smsNotifier.sent).toHaveLength(1);

    // Fifth bad PIN of second round triggers second SMS with PIN 11111
    service.claimReservation(7, 99999);
    expect(builder.smsNotifier.sent).toHaveLength(2);
  });
});
