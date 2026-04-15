from dataclasses import dataclass, field
from typing import List


DEFAULT_PAGE_NUMBER = 1
DEFAULT_PAGE_SIZE = 10
DEFAULT_TOTAL_ITEMS = 0


@dataclass(frozen=True)
class PageRequest:
    page_number: int
    page_size: int
    total_items: int
    total_pages: int = field(init=False)
    start_item: int = field(init=False)
    end_item: int = field(init=False)
    has_previous: bool = field(init=False)
    has_next: bool = field(init=False)

    def __post_init__(self) -> None:
        if self.total_items < 0:
            raise ValueError("totalItems must be >= 0")
        if self.page_size < 1:
            raise ValueError("pageSize must be >= 1")

        total_pages = (
            0 if self.total_items == 0
            else (self.total_items + self.page_size - 1) // self.page_size
        )
        page_number = _clamp_page_number(self.page_number, total_pages)
        start_item = 0 if self.total_items == 0 else (page_number - 1) * self.page_size + 1
        end_item = (
            0 if self.total_items == 0
            else min(page_number * self.page_size, self.total_items)
        )

        object.__setattr__(self, "page_number", page_number)
        object.__setattr__(self, "total_pages", total_pages)
        object.__setattr__(self, "start_item", start_item)
        object.__setattr__(self, "end_item", end_item)
        object.__setattr__(self, "has_previous", page_number > 1)
        object.__setattr__(self, "has_next", page_number < total_pages)

    def page_window(self, window_size: int) -> List[int]:
        if window_size <= 0 or self.total_pages == 0:
            return []
        size = min(window_size, self.total_pages)
        half = size // 2
        start = self.page_number - half
        end = start + size - 1
        if start < 1:
            start = 1
            end = size
        if end > self.total_pages:
            end = self.total_pages
            start = end - size + 1
        return list(range(start, end + 1))


def _clamp_page_number(requested: int, total_pages: int) -> int:
    if total_pages == 0:
        return 1
    if requested < 1:
        return 1
    if requested > total_pages:
        return total_pages
    return requested
