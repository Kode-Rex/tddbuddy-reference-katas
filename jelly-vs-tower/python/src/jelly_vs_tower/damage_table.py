from __future__ import annotations

from dataclasses import dataclass
from typing import TYPE_CHECKING

from .color_type import ColorType

if TYPE_CHECKING:
    from .jelly import Jelly
    from .random_source import RandomSource
    from .tower import Tower


@dataclass(frozen=True)
class DamageRange:
    min: int
    max: int


_TABLE: dict[tuple[ColorType, int, ColorType], DamageRange] = {
    # Blue Tower
    (ColorType.Blue, 1, ColorType.Blue): DamageRange(2, 5),
    (ColorType.Blue, 2, ColorType.Blue): DamageRange(5, 9),
    (ColorType.Blue, 3, ColorType.Blue): DamageRange(9, 12),
    (ColorType.Blue, 4, ColorType.Blue): DamageRange(12, 15),
    (ColorType.Blue, 1, ColorType.Red): DamageRange(0, 0),
    (ColorType.Blue, 2, ColorType.Red): DamageRange(1, 1),
    (ColorType.Blue, 3, ColorType.Red): DamageRange(2, 2),
    (ColorType.Blue, 4, ColorType.Red): DamageRange(3, 3),
    # Red Tower
    (ColorType.Red, 1, ColorType.Blue): DamageRange(0, 0),
    (ColorType.Red, 2, ColorType.Blue): DamageRange(1, 1),
    (ColorType.Red, 3, ColorType.Blue): DamageRange(2, 2),
    (ColorType.Red, 4, ColorType.Blue): DamageRange(3, 3),
    (ColorType.Red, 1, ColorType.Red): DamageRange(2, 5),
    (ColorType.Red, 2, ColorType.Red): DamageRange(5, 9),
    (ColorType.Red, 3, ColorType.Red): DamageRange(9, 12),
    (ColorType.Red, 4, ColorType.Red): DamageRange(12, 15),
    # BlueRed Tower
    (ColorType.BlueRed, 1, ColorType.Blue): DamageRange(2, 2),
    (ColorType.BlueRed, 2, ColorType.Blue): DamageRange(2, 4),
    (ColorType.BlueRed, 3, ColorType.Blue): DamageRange(4, 6),
    (ColorType.BlueRed, 4, ColorType.Blue): DamageRange(6, 8),
    (ColorType.BlueRed, 1, ColorType.Red): DamageRange(2, 2),
    (ColorType.BlueRed, 2, ColorType.Red): DamageRange(2, 4),
    (ColorType.BlueRed, 3, ColorType.Red): DamageRange(4, 6),
    (ColorType.BlueRed, 4, ColorType.Red): DamageRange(6, 8),
}


def _resolve_damage(tower: Tower, jelly_color: ColorType, random: RandomSource) -> int:
    r = _TABLE[(tower.color, tower.level, jelly_color)]
    return r.min if r.min == r.max else random.next(r.min, r.max)


def calculate_damage(tower: Tower, jelly: Jelly, random: RandomSource) -> int:
    if jelly.color == ColorType.BlueRed:
        blue_damage = _resolve_damage(tower, ColorType.Blue, random)
        red_damage = _resolve_damage(tower, ColorType.Red, random)
        return max(blue_damage, red_damage)

    return _resolve_damage(tower, jelly.color, random)
