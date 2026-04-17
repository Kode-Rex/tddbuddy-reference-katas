from .category import Category
from .money import Money

_PER_ITEM_LIMITS = {
    Category.MEALS: Money(50),
    Category.TRAVEL: Money(500),
    Category.ACCOMMODATION: Money(200),
    Category.EQUIPMENT: Money(1000),
    Category.OTHER: Money(100),
}

REPORT_MAXIMUM = Money(5000)
APPROVAL_THRESHOLD = Money(2500)


def limit_for(category: Category) -> Money:
    return _PER_ITEM_LIMITS[category]
