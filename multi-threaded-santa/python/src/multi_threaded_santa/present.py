from enum import Enum, auto


class PresentState(Enum):
    UNMADE = auto()
    MADE = auto()
    WRAPPED = auto()
    LOADED = auto()
    DELIVERED = auto()


class Present:
    def __init__(self, present_id: int) -> None:
        self.id = present_id
        self._state = PresentState.UNMADE

    @property
    def state(self) -> PresentState:
        return self._state

    def make(self) -> None:
        if self._state != PresentState.UNMADE:
            raise RuntimeError(f"Cannot make a present in state {self._state.name}.")
        self._state = PresentState.MADE

    def wrap(self) -> None:
        if self._state != PresentState.MADE:
            raise RuntimeError(f"Cannot wrap a present in state {self._state.name}.")
        self._state = PresentState.WRAPPED

    def load(self) -> None:
        if self._state != PresentState.WRAPPED:
            raise RuntimeError(f"Cannot load a present in state {self._state.name}.")
        self._state = PresentState.LOADED

    def deliver(self) -> None:
        if self._state != PresentState.LOADED:
            raise RuntimeError(f"Cannot deliver a present in state {self._state.name}.")
        self._state = PresentState.DELIVERED
