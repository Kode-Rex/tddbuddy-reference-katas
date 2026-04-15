from pagination import (
    DEFAULT_PAGE_NUMBER,
    DEFAULT_PAGE_SIZE,
    DEFAULT_TOTAL_ITEMS,
    PageRequest,
)


class PageRequestBuilder:
    def __init__(self) -> None:
        self._page_number = DEFAULT_PAGE_NUMBER
        self._page_size = DEFAULT_PAGE_SIZE
        self._total_items = DEFAULT_TOTAL_ITEMS

    def page_number(self, n: int) -> "PageRequestBuilder":
        self._page_number = n
        return self

    def page_size(self, n: int) -> "PageRequestBuilder":
        self._page_size = n
        return self

    def total_items(self, n: int) -> "PageRequestBuilder":
        self._total_items = n
        return self

    def build(self) -> PageRequest:
        return PageRequest(
            page_number=self._page_number,
            page_size=self._page_size,
            total_items=self._total_items,
        )
