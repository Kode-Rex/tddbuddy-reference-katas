class LimiterRuleInvalidError(Exception):
    """Raised when a Rule is constructed with a non-positive max_requests or window."""
    pass
