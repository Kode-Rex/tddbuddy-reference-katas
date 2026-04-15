class BookNotInCatalogError(Exception):
    """Raised when an operation references an ISBN not present in the library's catalog."""
    pass


class NoCopiesAvailableError(Exception):
    """Raised when a checkout is requested but no copy is available to the requesting member."""
    pass


class NoActiveLoanError(Exception):
    """Raised when a return is requested but the member has no active loan of the given ISBN."""
    pass
