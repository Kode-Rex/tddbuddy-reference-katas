class RegistrationRejectedError(Exception):
    """Raised when a would-be member fails to meet the registration invariants (e.g. minimum age)."""
    pass


class UnauthorizedError(Exception):
    """Raised when a non-admin user attempts an admin-only operation."""
    pass


class NoCopiesAvailableError(Exception):
    """Raised when a rental is requested but no copy of the title is available."""
    pass


class OverdueRentalError(Exception):
    """Raised when a user with an overdue rental attempts to rent another title."""
    pass


class NoActiveRentalError(Exception):
    """Raised when a return is requested but the user has no active rental of the given title."""
    pass


class TitleNotInCatalogError(Exception):
    """Raised when an operation references a title name not present in the club's catalog."""
    pass
