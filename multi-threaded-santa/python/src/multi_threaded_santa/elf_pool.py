class ElfPool:
    """Manages a fixed-size pool of elf workers.

    In Python, workers are threading.Thread instances.
    """

    def __init__(self, elf_count: int) -> None:
        if elf_count <= 0:
            raise ValueError("Elf pool must have at least one elf.")
        self.elf_count = elf_count
