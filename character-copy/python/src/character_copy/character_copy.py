from __future__ import annotations

from .destination import Destination
from .source import Source


def copy(source: Source, destination: Destination) -> None:
    while True:
        ch = source.read()
        if ch == "\n":
            return
        destination.write(ch)
