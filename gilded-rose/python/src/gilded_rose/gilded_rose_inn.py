from .inventory import Inventory


class GildedRoseInn:
    def __init__(self, inventory: Inventory) -> None:
        self.inventory = inventory

    def update_inventory(self) -> None:
        for item in self.inventory.items:
            item.quality -= 1
