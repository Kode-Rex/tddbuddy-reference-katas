class AccountNotOpenException(Exception):
    pass


class AccountClosedException(Exception):
    pass


class InsufficientFundsException(Exception):
    pass


class InvalidAmountException(Exception):
    pass


class NonZeroBalanceException(Exception):
    pass
