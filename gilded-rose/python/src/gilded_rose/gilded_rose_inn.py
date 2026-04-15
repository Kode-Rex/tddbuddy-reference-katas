from .category import Category
from .inventory import Inventory


class GildedRoseInn:
    def __init__(self, inventory: Inventory) -> None:
        self.inventory = inventory

    def update_inventory(self) -> None:
        for item in self.inventory.items:
            if item.category == Category.AGED:
                item.quality += 1
            else:
                degrade = 2 if item.sell_in <= 0 else 1
                item.quality = max(0, item.quality - degrade)
            item.sell_in -= 1
