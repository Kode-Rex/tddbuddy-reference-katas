# Gilded Rose — Scenarios

This is the shared specification that all three language implementations (C#, TypeScript, Python) satisfy. Test names in every language map 1:1 to the scenario names below.

## Domain Rules (from the kata brief)

The Gilded Rose Inn sells items. Every day the `UpdateInventory()` operation runs, adjusting each item's **quality** and **sell-in** values according to category-specific rules.

### Invariants (all items)

- Quality is never negative
- Quality never exceeds 50 (legendary items are the exception — see below)

### Standard Items

- Quality decreases by 1 each day
- Once the sell-by date has passed, quality decreases by 2 each day
- Sell-in decreases by 1 each day

### Aged Items (e.g., Aged Brie)

- Quality *increases* by 1 each day
- Once the sell-by date has passed, quality increases by 2 each day
- Sell-in decreases by 1 each day

### Legendary Items (e.g., Sulfuras)

- Quality never changes (always 80)
- Sell-in never changes
- Not subject to the 0–50 quality invariant

### Backstage Passes

- Quality increases by 1 each day
- Quality increases by 2 when there are 10 days or fewer until the concert
- Quality increases by 3 when there are 5 days or fewer until the concert
- Quality drops to 0 after the concert (sell-in is negative)
- Sell-in decreases by 1 each day

### Conjured Items

- Degrade in quality twice as fast as standard items
- Quality decreases by 2 each day
- Once the sell-by date has passed, quality decreases by 4 each day
- Sell-in decreases by 1 each day

## Test Scenarios

Each scenario below is a test. Test names in all three languages match these scenario titles exactly (modulo language naming conventions: `snake_case` in Python, `Test_Case_Naming` in C# with underscores, `camelCase` or sentence-style in TypeScript).

### Standard Items

1. **Standard items lose one quality per day while fresh**
2. **Standard items lose two quality per day after the sell-by date**
3. **Standard item quality never goes below zero**
4. **Standard item sell-in decreases by one each day**

### Aged Items

5. **Aged items gain one quality per day while fresh**
6. **Aged items gain two quality per day after the sell-by date**
7. **Aged item quality never exceeds fifty**
8. **Aged item sell-in decreases by one each day**

### Legendary Items

9. **Legendary items never lose quality**
10. **Legendary item sell-in never changes**
11. **Legendary items may have quality above fifty**

### Backstage Passes

12. **Backstage pass quality increases by one when concert is more than ten days away**
13. **Backstage pass quality increases by two when concert is ten days or fewer away**
14. **Backstage pass quality increases by three when concert is five days or fewer away**
15. **Backstage pass quality drops to zero after the concert**
16. **Backstage pass quality never exceeds fifty before the concert**

### Conjured Items

17. **Conjured items lose two quality per day while fresh**
18. **Conjured items lose four quality per day after the sell-by date**
19. **Conjured item quality never goes below zero**

### Multi-Item Worlds

20. **Mixed inventory: each item follows its own category rules on the same day**
21. **Multi-day aging: ten days of updates applied in sequence produce correct quality progression**

## Ubiquitous Vocabulary

These terms appear in the kata brief and must flow into test names, builder methods, and domain types in every language:

| Term | Meaning |
|------|---------|
| **Inn** | The shop (`GildedRoseInn`) |
| **Inventory** | The collection of items (`Inventory`, `UpdateInventory()`) |
| **Item** | A thing that can be sold; has a name, category, quality, and sell-in |
| **Quality** | How valuable an item is to a buyer (0–50 normally) |
| **Sell-in** | Days remaining until the sell-by date; goes negative after it passes |
| **Standard** | The default item category |
| **Aged** | Gets better over time (Brie-like) |
| **Legendary** | Never degrades, never sells (Sulfuras-like) |
| **Backstage Pass** | Increases in value as the concert approaches, worthless after |
| **Conjured** | Degrades faster than standard |
| **Fresh** | Sell-in > 0 |
| **Expired** / **Past sell-by** | Sell-in ≤ 0 |

## Scenario-to-Builder Mapping

The builder API should make these scenarios trivial to express:

```
anItem()                       // standard, quality 20, sell-in 10
aStandardItem()                // same as anItem
anAgedItem()                   // "Aged Brie", aged category
aLegendaryItem()               // "Sulfuras, Hand of Ragnaros", quality 80
aBackstagePass()               // "Backstage passes to a TAFKAL80ETC concert"
aConjuredItem()                // conjured category

// Fluent overrides
anItem().withQuality(...)
anItem().withSellIn(...)
anAgedItem().pastSellBy()              // convenience for sell-in < 0
aBackstagePass().withConcertIn(3)      // convenience for sell-in
```

The test for scenario 13 should read:

```
Backstage_pass_quality_increases_by_two_when_concert_is_ten_days_or_fewer_away:
    let pass = aBackstagePass().withConcertIn(10).withQuality(20)
    inventory.updateOneDay()
    pass.quality.should equal 22
```

Not:

```
TestBackstage_10Days:
    let item = new Item("Backstage passes to a TAFKAL80ETC concert", 10, 20)
    updateQuality(item)
    assert item.quality == 22
```

Same test. Very different artifact.
