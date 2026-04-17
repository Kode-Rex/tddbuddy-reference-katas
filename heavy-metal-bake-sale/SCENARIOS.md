# Heavy Metal Bake Sale — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Product** | A bake-sale item with a name, price, purchase code, and initial stock quantity |
| **PurchaseCode** | A single-letter code identifying a product (`B`, `M`, `C`, `W`) |
| **Money** | A monetary amount with two decimal precision; the domain never uses raw numbers |
| **Inventory** | The collection of products available for sale, each with a stock count |
| **Order** | A request to purchase one or more products, specified as a comma-delimited string of purchase codes |
| **BakeSale** | The aggregate that owns the inventory, processes orders, and calculates change |

## Domain Rules

- The bake sale starts with a **fixed inventory** of four products:

  | Item | Price | Quantity | Purchase Code |
  |------|-------|----------|---------------|
  | Brownie | $0.75 | 48 | B |
  | Muffin | $1.00 | 36 | M |
  | Cake Pop | $1.35 | 24 | C |
  | Water | $1.50 | 30 | W |

- Orders are specified as a **comma-delimited string** of purchase codes (e.g. `"B,C,W"`)
- Each code in the order represents **one unit** of that product
- The **total** is the sum of prices for all items in the order
- If **any item** in the order is out of stock, the entire order is rejected with `"<Name> is out of stock"`
- A successful order **decrements** stock for each purchased item
- After calculating the total, the customer pays an amount:
  - If the payment is **greater than or equal to** the total, change is calculated as `payment - total`
  - If the payment is **less than** the total, the payment is rejected with `"Not enough money."`
- An **unknown purchase code** is rejected with `"Unknown purchase code: <code>"`
- Multiple units of the same product can appear in one order (e.g. `"B,B"` buys two brownies)

## Test Scenarios

### Product Setup

1. **A product has a name, price, purchase code, and stock quantity**
2. **Default inventory contains brownie, muffin, cake pop, and water**

### Order Totals

3. **Single brownie order totals $0.75**
4. **Single muffin order totals $1.00**
5. **Single cake pop order totals $1.35**
6. **Single water order totals $1.50**
7. **Multiple different items total to the sum of their prices**
8. **Duplicate items in an order are each counted separately**

### Stock Management

9. **Successful order decrements stock for each purchased item**
10. **Out of stock item rejects the order with item name**
11. **Partially stocked order where second item is out of stock rejects with that item name**
12. **Order does not decrement stock when any item is out of stock**

### Payment and Change

13. **Exact payment returns zero change**
14. **Overpayment returns correct change**
15. **Underpayment is rejected with not enough money**

### Edge Cases

16. **Unknown purchase code is rejected**
17. **Multiple items with one out of stock reports the out of stock item**
18. **Buying all remaining stock of an item succeeds**
19. **Buying one more after stock is depleted fails**
