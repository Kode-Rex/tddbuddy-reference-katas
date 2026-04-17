# Jelly vs Tower — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **ColorType** | One of `Blue`, `Red`, or `BlueRed` — classifies both towers and jellies |
| **Jelly** | An entity with an identifier, color, and health; dies when health reaches zero |
| **Tower** | An entity with an identifier, color, level (1--4), and a damage table lookup |
| **DamageTable** | The lookup that maps (tower color, tower level, jelly color) to a damage range |
| **RandomSource** | Collaborator that picks a value within a damage range — injected so tests control randomness |
| **CombatLog** | A record of one attack: which tower hit which jelly for how much damage |
| **Arena** | Orchestrates combat rounds: towers attack, damage applies, dead jellies are removed |

## Domain Rules

- Towers and jellies both have a **ColorType**: `Blue`, `Red`, or `BlueRed`
- Towers have a **level** from 1 to 4; levels outside this range are rejected
- Jelly **health** must be strictly positive at creation
- A jelly is **alive** when health > 0 and **dead** when health <= 0
- Damage is looked up from the damage table by (tower color, tower level, jelly color)
- Damage ranges are resolved by the injectable **RandomSource**
- A **BlueRed jelly** takes the **higher** of the Blue-column and Red-column damage
- Dead jellies **cannot be attacked** — towers skip them
- If **no alive jellies** remain, tower attacks produce no effect and no log entry
- Each combat round: every tower attacks the **first alive jelly**, damage is applied, dead jellies are removed, and a combat log is produced

## Damage Table

### Blue Tower

| Level | vs Blue Jelly | vs Red Jelly |
|-------|---------------|--------------|
| 1 | 2--5 | 0 |
| 2 | 5--9 | 1 |
| 3 | 9--12 | 2 |
| 4 | 12--15 | 3 |

### Red Tower

| Level | vs Blue Jelly | vs Red Jelly |
|-------|---------------|--------------|
| 1 | 0 | 2--5 |
| 2 | 1 | 5--9 |
| 3 | 2 | 9--12 |
| 4 | 3 | 12--15 |

### BlueRed Tower

| Level | vs Blue Jelly | vs Red Jelly |
|-------|---------------|--------------|
| 1 | 2 | 2 |
| 2 | 2--4 | 2--4 |
| 3 | 4--6 | 4--6 |
| 4 | 6--8 | 6--8 |

## Test Scenarios

### Jelly Creation and Health

1. **Jelly starts alive with the given health**
2. **Jelly with zero health is rejected**
3. **Jelly with negative health is rejected**
4. **Jelly dies when health reaches zero**
5. **Jelly dies when health drops below zero**

### Tower Creation and Validation

6. **Tower is created with a color and level**
7. **Tower with level below 1 is rejected**
8. **Tower with level above 4 is rejected**

### Damage Lookup — Blue Tower

9. **Blue tower level 1 deals 2 to 5 damage to a blue jelly**
10. **Blue tower level 1 deals 0 damage to a red jelly**
11. **Blue tower level 4 deals 12 to 15 damage to a blue jelly**
12. **Blue tower level 2 deals 1 damage to a red jelly**

### Damage Lookup — Red Tower

13. **Red tower level 3 deals 9 to 12 damage to a red jelly**
14. **Red tower level 2 deals 1 damage to a blue jelly**
15. **Red tower level 1 deals 0 damage to a blue jelly**

### Damage Lookup — BlueRed Tower

16. **BlueRed tower level 4 deals 6 to 8 damage to a blue jelly**
17. **BlueRed tower level 4 deals 6 to 8 damage to a red jelly**
18. **BlueRed tower level 1 deals 2 damage to a blue jelly**

### BlueRed Jelly — Takes Higher Column

19. **BlueRed jelly takes the higher of blue and red column damage**
20. **BlueRed jelly hit by BlueRed tower uses both columns and takes the higher**

### Combat Flow

21. **Tower attacks the first alive jelly and produces a combat log**
22. **Dead jellies are skipped — tower attacks the next alive jelly**
23. **Tower attack does nothing when no alive jellies remain**
24. **Multiple towers each attack in a single round**
25. **Jelly killed during a round is removed before the next tower attacks**
