# Zombie Survivor — Scenarios

Shared specification satisfied by the C#, TypeScript, and Python implementations.

## Ubiquitous Vocabulary

| Term | Meaning |
|------|---------|
| **Survivor** | A named character with wounds, equipment, experience, actions, and skills |
| **Game** | The aggregate root; owns survivors, enforces unique names, tracks game level and history |
| **Wound** | A hit taken by a survivor; 2 wounds = death |
| **Equipment** | An item carried by a survivor, either "In Hand" (max 2) or "In Reserve" (remaining capacity) |
| **Level** | Experience tier: Blue (0+), Yellow (7+), Orange (19+), Red (43+) |
| **Skill** | An ability unlocked at level thresholds: +1 Action, Hoard, Sniper, Tough |
| **History** | An append-only event log recording every significant game event with a timestamp |
| **HistoryEntry** | A single event in the history log with a timestamp and description |

## Domain Rules

- A new survivor starts with **0 wounds**, **0 experience**, **3 actions**, level **Blue**, alive
- A survivor **dies** on their **2nd wound**; additional wounds to a dead survivor are ignored
- Survivors carry up to **5 pieces** of equipment; up to **2 in hand**, the rest in reserve
- Each wound **reduces carrying capacity by 1**; excess equipment must be discarded
- A game starts with **0 survivors** and survivors can be added at any time
- Survivor **names must be unique** within a game
- The game **ends when all survivors are dead**
- Survivors earn **1 experience** per zombie killed
- The **game level** equals the highest level among living survivors
- History records: game started, survivor added, equipment acquired, wound received, survivor died, level up, game level changed, game ended
- At **Yellow**, the survivor gains the mandatory **+1 Action** skill
- At **Orange**, the survivor chooses **1 of 2** available skills
- At **Red**, the survivor chooses **1 of 3** available skills
- Available skills: **+1 Action**, **Hoard** (+1 equipment capacity), **Sniper** (flavor), **Tough** (flavor)

## Test Scenarios

### Survivors

1. **New survivor has zero wounds**
2. **New survivor has three actions per turn**
3. **New survivor is alive**
4. **New survivor starts at level Blue**
5. **Receiving a wound leaves the survivor alive with one wound**
6. **Receiving a second wound kills the survivor**
7. **Wounding a dead survivor has no effect**

### Equipment

8. **New survivor can carry up to five pieces of equipment**
9. **Survivor can hold up to two items in hand**
10. **Remaining equipment goes in reserve**
11. **Equipping a sixth item is rejected**
12. **One wound reduces carrying capacity to four**
13. **Wound with full equipment requires discarding one item**

### Game

14. **New game starts with zero survivors**
15. **Adding a survivor increases the survivor count**
16. **Adding a survivor with a duplicate name is rejected**
17. **Game ends when all survivors are dead**

### Experience and Levels

18. **Killing a zombie awards one experience point**
19. **Survivor with seven experience is level Yellow**
20. **Survivor with nineteen experience is level Orange**
21. **Survivor with forty-three experience is level Red**
22. **Game level matches the highest level among living survivors**

### History

23. **New game records a game-started event**
24. **Adding a survivor records a survivor-added event**
25. **Receiving a wound records a wound-received event**
26. **Survivor death records a survivor-died event**
27. **Leveling up records a level-up event**
28. **Game level change records a game-level-changed event**
29. **Game ending records a game-ended event**

### Skills

30. **Reaching Yellow unlocks plus-one-action as the mandatory first skill**
31. **Plus-one-action skill increases actions to four**
32. **Hoard skill increases equipment capacity by one**
