from calc_refactor import Calculator


class CalculatorBuilder:
    def __init__(self) -> None:
        self._keys: str = ""

    def press_keys(self, keys: str) -> "CalculatorBuilder":
        self._keys += keys
        return self

    def build(self) -> Calculator:
        calculator = Calculator()
        for key in self._keys:
            calculator.press(key)
        return calculator


def a_calculator() -> CalculatorBuilder:
    return CalculatorBuilder()
