# Supermarket Pricing — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Product** | A catalog entry with a SKU, name, and pricing rule |
| **PricingRule** | A strategy that computes the charge for a scanned quantity or weight (`UnitPrice`, `MultiBuy`, `BuyOneGetOneFree`, `WeightedPrice`, `ComboDeal`) |
| **Checkout** | The aggregate that accumulates scanned items and computes the total |
| **Money** | A monetary amount in cents; the domain never uses raw floats for prices |
| **Weight** | A non-negative decimal amount in kilograms |
| **SKU** | A string identifier for a product (e.g. `"A"`, `"Bananas"`) |

## Domain Rules

- A checkout starts with a **zero** total
- Scanning an item increments its count (or adds weight for weighted items)
- The total is the sum of each product's charge, computed by its pricing rule
- **UnitPrice(cents)** — each item costs a fixed amount
- **MultiBuy(n, totalCents)** — every group of `n` items costs `totalCents`; remaining items at unit price
- **BuyOneGetOneFree** — every second item is free (equivalent to buy 1 get 1 free)
- **WeightedPrice(centsPerKg)** — charge = weight in kg × price per kg, **rounded to the nearest cent**
- **ComboDeal(skuA, skuB, dealCents)** — when one of each SKU is present, the pair costs `dealCents` instead of the sum of their individual prices; applies once per qualifying pair
- Scanning order does **not** affect the total
- Multiple pricing rules can coexist on different products in the same checkout
- A product with no special rule uses plain unit pricing

## Test Scenarios

### Simple Pricing

1. **Empty checkout has a zero total**
2. **Scanning a single item returns its unit price**
3. **Scanning two different items returns the sum of their unit prices**
4. **Scanning the same item twice returns double its unit price**

### Multi-Buy Discounts

5. **Three A's at three-for-130 costs 130**
6. **Four A's at three-for-130 costs 180** (one group of 3 at 130, plus one at 50)
7. **Two B's at two-for-45 costs 45**
8. **Three B's at two-for-45 costs 75** (one group of 2 at 45, plus one at 30)
9. **Mixed basket with multi-buy discounts totals correctly** (A×3 + B×2 = 130 + 45 = 175)
10. **Scanning order does not affect multi-buy total** (A, B, A, B, A = 175)

### Buy One Get One Free

11. **Two C's with BOGOF costs 20** (second one free)
12. **Three C's with BOGOF costs 40** (pairs: 1 paid + 1 free, then 1 paid)
13. **Single C with BOGOF costs 20** (no discount without a pair)

### Weighted Items

14. **Bananas at 199 cents per kg for 0.5 kg costs 100** (99.5 rounded to 100)
15. **Apples at 349 cents per kg for 1.0 kg costs 349**
16. **Weighted item with zero weight costs zero**

### Combo Deals

17. **D plus C together at combo price costs 25** (instead of 15 + 20 = 35)
18. **D plus C plus D uses combo once, remaining D at unit price** (25 + 15 = 40)
19. **D alone with a combo deal configured still costs unit price**
20. **C alone with a combo deal configured still costs unit price**
