class BreakerThresholdInvalidError(Exception):
    """Raised when a breaker is constructed with a non-positive failure threshold."""
    pass


class BreakerTimeoutInvalidError(Exception):
    """Raised when a breaker is constructed with a non-positive reset timeout."""
    pass


class CircuitOpenError(Exception):
    """Raised by execute when the breaker is open and declines to invoke the operation."""
    pass
