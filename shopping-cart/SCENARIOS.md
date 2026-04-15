# Shopping Cart — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Cart** | The aggregate root (`Cart`); owns a list of `LineItem`s |
| **Product** | A catalog entry (`Product`) with a SKU, name, and unit price |
| **LineItem** | A product-plus-quantity pair (`LineItem`) held in the cart |
| **Money** | A monetary amount with two decimal precision; the domain never uses raw numbers |
| **Quantity** | A positive whole number; the domain rejects zero and negative counts |
| **DiscountPolicy** | A strategy that adjusts a line's price (`Percent`, `FixedOff`, `BuyXGetY`, `Bulk`) |

## Domain Rules

- A cart starts **empty**
- Adding a product sets the line's quantity to **1** if the product isn't already in the cart
- Adding a product that **is already in the cart** increments the existing line's quantity
- Removing a product removes its line entirely (not just decrements)
- Updating a quantity to a **positive** integer replaces the line's quantity
- Updating a quantity to **zero or negative** is rejected
- **Line subtotal** = unit price × quantity, then its discount policy applies
- **Cart total** = sum of line subtotals
- Discounts never take a line below **zero**

### Discount Policies

- **Percent(p%)** — reduces subtotal by `p%` (e.g. 10% off)
- **FixedOff(amount)** — subtracts a flat amount per line (clamped at zero)
- **BuyXGetY(x, y)** — for every group of `x+y`, `y` items are free
- **Bulk(threshold, unitPrice)** — when quantity ≥ threshold, use a special lower unit price

## Test Scenarios

### Basic Cart Operations

1. **New cart is empty**
2. **Adding a product adds one line item with quantity one**
3. **Adding the same product twice increments the existing line's quantity**
4. **Removing a product removes its line item**
5. **Updating quantity to a positive number replaces the line's quantity**
6. **Updating quantity to zero is rejected**
7. **Updating quantity to a negative number is rejected**

### Subtotals and Totals

8. **Line subtotal is unit price multiplied by quantity**
9. **Cart total is the sum of line subtotals**
10. **Cart total of an empty cart is zero**

### Percent Discount

11. **Ten percent off reduces the line subtotal by ten percent**
12. **Percent discount applies before cart total is summed**

### Fixed Discount

13. **Fixed discount subtracts a flat amount from the line subtotal**
14. **Fixed discount cannot take a line subtotal below zero**

### Buy X Get Y Free

15. **Buy two get one free charges only for two when three are bought**
16. **Buy two get one free charges for four when five are bought**

### Bulk Pricing

17. **Bulk pricing applies the lower unit price once the threshold is reached**
18. **Bulk pricing does not apply below the threshold**
