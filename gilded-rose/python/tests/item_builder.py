from __future__ import annotations

from gilded_rose import Category, Item


class ItemBuilder:
    def __init__(self) -> None:
        self._name: str = "Elixir of the Mongoose"
        self._category: Category = Category.STANDARD
        self._quality: int = 10
        self._sell_in: int = 5

    def standard(self) -> ItemBuilder:
        self._category = Category.STANDARD
        return self

    def aged(self) -> ItemBuilder:
        self._category = Category.AGED
        return self

    def legendary(self) -> ItemBuilder:
        self._category = Category.LEGENDARY
        return self

    def backstage_pass(self) -> ItemBuilder:
        self._category = Category.BACKSTAGE_PASS
        return self

    def conjured(self) -> ItemBuilder:
        self._category = Category.CONJURED
        return self

    def named(self, name: str) -> ItemBuilder:
        self._name = name
        return self

    def with_quality(self, quality: int) -> ItemBuilder:
        self._quality = quality
        return self

    def with_sell_in(self, sell_in: int) -> ItemBuilder:
        self._sell_in = sell_in
        return self

    def build(self) -> Item:
        return Item(self._name, self._category, self._quality, self._sell_in)
