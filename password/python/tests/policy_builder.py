from password import DEFAULT_MIN_LENGTH, Policy


class PolicyBuilder:
    def __init__(self) -> None:
        self._min_length = DEFAULT_MIN_LENGTH
        self._requires_digit = False
        self._requires_symbol = False
        self._requires_upper = False
        self._requires_lower = False

    def min_length(self, n: int) -> "PolicyBuilder":
        self._min_length = n
        return self

    def requires_digit(self) -> "PolicyBuilder":
        self._requires_digit = True
        return self

    def requires_symbol(self) -> "PolicyBuilder":
        self._requires_symbol = True
        return self

    def requires_upper(self) -> "PolicyBuilder":
        self._requires_upper = True
        return self

    def requires_lower(self) -> "PolicyBuilder":
        self._requires_lower = True
        return self

    def build(self) -> Policy:
        return Policy(
            min_length=self._min_length,
            requires_digit=self._requires_digit,
            requires_symbol=self._requires_symbol,
            requires_upper=self._requires_upper,
            requires_lower=self._requires_lower,
        )
