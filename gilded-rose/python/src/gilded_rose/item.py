from dataclasses import dataclass

from .category import Category


@dataclass
class Item:
    name: str
    category: Category
    quality: int
    sell_in: int
