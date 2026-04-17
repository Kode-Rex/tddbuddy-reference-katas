import type { Reservation } from '../src/Reservation.js';
import type { ReservationRepository } from '../src/ReservationRepository.js';

export class InMemoryReservationRepository implements ReservationRepository {
  readonly all: Reservation[] = [];

  save(reservation: Reservation): void {
    const idx = this.all.findIndex(r => r.id === reservation.id);
    if (idx >= 0) {
      this.all[idx] = reservation;
    } else {
      this.all.push(reservation);
    }
  }

  findActiveByCustomerEmail(email: string): Reservation | undefined {
    return this.all.find(r => r.customer.email === email && !r.isUsed);
  }

  findActiveByMachineNumber(machineNumber: number): Reservation | undefined {
    return this.all.find(r => r.machineNumber === machineNumber && !r.isUsed);
  }
}
