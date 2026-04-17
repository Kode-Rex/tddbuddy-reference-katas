from __future__ import annotations

from laundry_reservation.reservation import Reservation


class InMemoryReservationRepository:
    def __init__(self) -> None:
        self.all: list[Reservation] = []

    def save(self, reservation: Reservation) -> None:
        for i, r in enumerate(self.all):
            if r.id == reservation.id:
                self.all[i] = reservation
                return
        self.all.append(reservation)

    def find_active_by_customer_email(self, email: str) -> Reservation | None:
        return next(
            (r for r in self.all if r.customer.email == email and not r.is_used),
            None,
        )

    def find_active_by_machine_number(self, machine_number: int) -> Reservation | None:
        return next(
            (r for r in self.all if r.machine_number == machine_number and not r.is_used),
            None,
        )
