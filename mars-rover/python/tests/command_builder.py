class CommandBuilder:
    def __init__(self) -> None:
        self._buffer = ""

    def forward(self) -> "CommandBuilder":
        self._buffer += "F"
        return self

    def backward(self) -> "CommandBuilder":
        self._buffer += "B"
        return self

    def left(self) -> "CommandBuilder":
        self._buffer += "L"
        return self

    def right(self) -> "CommandBuilder":
        self._buffer += "R"
        return self

    def build(self) -> str:
        return self._buffer
