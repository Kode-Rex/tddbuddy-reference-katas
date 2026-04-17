from __future__ import annotations


class FixedMachineSelector:
    def __init__(self, machine_number: int) -> None:
        self._machine_number = machine_number

    def select_available(self) -> int:
        return self._machine_number
