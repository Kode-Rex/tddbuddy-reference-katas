class OutOfStockException(Exception):
    def __init__(self, product_name: str) -> None:
        super().__init__(f"{product_name} is out of stock")


class InsufficientPaymentException(Exception):
    def __init__(self) -> None:
        super().__init__("Not enough money.")


class UnknownPurchaseCodeException(Exception):
    def __init__(self, code: str) -> None:
        super().__init__(f"Unknown purchase code: {code}")
