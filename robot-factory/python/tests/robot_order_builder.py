from __future__ import annotations

from robot_factory import PartOption, PartType, RobotOrder


class RobotOrderBuilder:
    def __init__(self) -> None:
        self._head: PartOption = PartOption.STANDARD_VISION
        self._body: PartOption = PartOption.SQUARE
        self._arms: PartOption = PartOption.HANDS
        self._movement: PartOption = PartOption.WHEELS
        self._power: PartOption = PartOption.SOLAR
        self._excluded: set[PartType] = set()

    def with_head(self, option: PartOption) -> RobotOrderBuilder:
        self._head = option
        return self

    def with_body(self, option: PartOption) -> RobotOrderBuilder:
        self._body = option
        return self

    def with_arms(self, option: PartOption) -> RobotOrderBuilder:
        self._arms = option
        return self

    def with_movement(self, option: PartOption) -> RobotOrderBuilder:
        self._movement = option
        return self

    def with_power(self, option: PartOption) -> RobotOrderBuilder:
        self._power = option
        return self

    def without(self, part_type: PartType) -> RobotOrderBuilder:
        self._excluded.add(part_type)
        return self

    def build(self) -> RobotOrder:
        order = RobotOrder()
        if PartType.HEAD not in self._excluded:
            order.configure(PartType.HEAD, self._head)
        if PartType.BODY not in self._excluded:
            order.configure(PartType.BODY, self._body)
        if PartType.ARMS not in self._excluded:
            order.configure(PartType.ARMS, self._arms)
        if PartType.MOVEMENT not in self._excluded:
            order.configure(PartType.MOVEMENT, self._movement)
        if PartType.POWER not in self._excluded:
            order.configure(PartType.POWER, self._power)
        return order
