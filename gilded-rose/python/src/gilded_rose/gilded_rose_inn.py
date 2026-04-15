from .category import Category
from .inventory import Inventory


class GildedRoseInn:
    def __init__(self, inventory: Inventory) -> None:
        self.inventory = inventory

    def update_inventory(self) -> None:
        for item in self.inventory.items:
            if item.category == Category.LEGENDARY:
                continue
            if item.category == Category.AGED:
                gain = 2 if item.sell_in <= 0 else 1
                item.quality = min(50, item.quality + gain)
            elif item.category == Category.BACKSTAGE_PASS:
                if item.sell_in <= 0:
                    item.quality = 0
                else:
                    if item.sell_in <= 5:
                        gain = 3
                    elif item.sell_in <= 10:
                        gain = 2
                    else:
                        gain = 1
                    item.quality = min(50, item.quality + gain)
            elif item.category == Category.CONJURED:
                degrade = 4 if item.sell_in <= 0 else 2
                item.quality = max(0, item.quality - degrade)
            else:
                degrade = 2 if item.sell_in <= 0 else 1
                item.quality = max(0, item.quality - degrade)
            item.sell_in -= 1
