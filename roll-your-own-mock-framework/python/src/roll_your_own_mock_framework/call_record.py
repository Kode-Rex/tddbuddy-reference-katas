from dataclasses import dataclass, field
from typing import Any


@dataclass(frozen=True)
class CallRecord:
    method_name: str
    args: tuple[Any, ...] = field(default_factory=tuple)
