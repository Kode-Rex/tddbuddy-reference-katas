import type { Reservation } from './Reservation.js';

export interface ReservationRepository {
  save(reservation: Reservation): void;
  findActiveByCustomerEmail(email: string): Reservation | undefined;
  findActiveByMachineNumber(machineNumber: number): Reservation | undefined;
}
