class UnknownColumnError(Exception):
    def __init__(self, column_name: str) -> None:
        super().__init__(f"Unknown column: {column_name}")
