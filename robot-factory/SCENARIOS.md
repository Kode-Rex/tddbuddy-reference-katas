# Robot Factory — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **PartType** | One of the five configurable slots on a robot: Head, Body, Arms, Movement, Power |
| **PartOption** | A specific choice within a part type (e.g. "Infrared Vision" for Head) |
| **RobotOrder** | A complete configuration specifying one option per part type |
| **PartSupplier** | An external service that can quote and sell parts; injected as a collaborator |
| **PartQuote** | A supplier's price for a specific part option |
| **CostBreakdown** | The result of costing: one cheapest quote per part type, plus total |
| **Money** | A positive monetary amount; never a bare number |
| **RobotFactory** | The aggregate that coordinates suppliers to cost and purchase robots |

## Domain Rules

- A robot order must specify exactly **one option per part type** (Head, Body, Arms, Movement, Power)
- An order missing any part type is **incomplete** and rejected before costing
- The factory queries **all registered suppliers** for each required part
- For each part type, the factory selects the **cheapest available quote**
- If **no supplier** carries a required part option, the factory rejects with "Part not available: {PartOption}"
- **Cost Robot** returns a breakdown of the cheapest quote per part type and the total cost
- **Purchase Robot** calls `purchase` on the winning supplier for each part; returns the purchased parts
- A supplier may carry some part options but not all — the factory must handle partial catalogs
- At least **three suppliers** must be registered (enforced in tests, not the factory itself)

## Test Scenarios

### Order Validation

1. **Order missing a part type is rejected as incomplete**
2. **Order with all five part types is accepted**

### Costing — Single Supplier

3. **Cost returns the supplier's price for each part**
4. **Cost total is the sum of all part prices**

### Costing — Multiple Suppliers

5. **Cost selects the cheapest quote when two suppliers carry the same part**
6. **Cost selects parts from different suppliers when each is cheapest for different parts**
7. **Cost breakdown shows the winning supplier for each part**

### Costing — Partial Catalogs

8. **Cost succeeds when a part is available from only one of three suppliers**
9. **Cost fails with part not available when no supplier carries a required part**

### Costing — Edge Cases

10. **Cost with identical prices from two suppliers picks either (deterministic)**
11. **Cost with three suppliers each cheapest for different parts**

### Purchasing

12. **Purchase calls the winning supplier's purchase method for each part**
13. **Purchase returns the list of purchased parts with their suppliers**
14. **Purchase is rejected when the order is incomplete**
15. **Purchase fails when a part is not available from any supplier**

### Supplier Behavior

16. **Supplier that does not carry a part returns no quote**
17. **Supplier returns a quote for a part it carries**
18. **Supplier purchase records the transaction**

### Full Assembly

19. **Full robot with three suppliers — each cheapest for some parts — costs correctly**
20. **Full robot purchased from mixed suppliers — each part from its cheapest source**
