class LineItemNotFoundError(Exception):
    """Raised when a cart operation references a SKU that has no matching line item."""
    pass
