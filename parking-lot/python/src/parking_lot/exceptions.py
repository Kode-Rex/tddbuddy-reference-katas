class VehicleAlreadyParkedError(Exception):
    """Raised when attempting to park a vehicle that is already in the lot."""
    pass


class NoAvailableSpotError(Exception):
    """Raised when no compatible spot is available for the vehicle."""
    pass


class InvalidTicketError(Exception):
    """Raised when a ticket is not valid for exit."""
    pass


class InvalidLotConfigurationError(Exception):
    """Raised when a lot is constructed with invalid configuration."""
    pass
