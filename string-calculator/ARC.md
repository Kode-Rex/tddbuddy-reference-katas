# String Calculator — Teaching Arc

The intended commit sequence all three language implementations follow. Language idioms diverge at the line level, but the arc — what lands at what gear, where the reflections sit — is shared.

Legend: **R** red, **G** green, **F** refactor, **Ref** reflect (empty commit), **S** spec-on-arrival (test passes immediately because a prior triangulation generalized).

| # | Phase | Gear | What |
|---|-------|------|------|
| 1 | scaffold | — | Empty solution/project, no tests. |
| 2 | R | low | Empty string returns zero. |
| 3 | G (fake-it) | low | `return 0`. Restrained from writing more. |
| 4 | R | low | Single number returns itself. |
| 5 | G | low | Parse one int. |
| 6 | R | low | Two numbers return sum. |
| 7 | G | low | Split on comma, sum ints. |
| 8 | F | low | Extract parse-and-sum helper. |
| 9 | Ref | low→middle | Triangulation complete; arbitrary count already works. |
| 10 | S | middle | Many numbers returns sum — passes on arrival. |
| 11 | R | middle | Newline as delimiter. |
| 12 | G | middle | Normalize newlines to commas before split. |
| 13 | Ref | middle | Gear shift. The shape of the function is clear; next scenarios go red+green in one commit. |
| 14 | R+G | middle | Custom single-char delimiter `//;\n1;2`. |
| 15 | F | middle | Extract delimiter parser — header parsing grows; isolate it. |
| 16 | R+G | middle | Negative throws `"negatives not allowed: -1"`. |
| 17 | R+G | middle | Multiple negatives listed `"negatives not allowed: -1, -2"`. |
| 18 | R+G | middle | Numbers greater than 1000 are ignored. |
| 19 | R+G | high | Any-length delimiter `//[***]\n1***2***3`. Regex in the parser; the rule is data. |
| 20 | R+G | high | Multiple single-char delimiters `//[*][%]\n1*2%3`. |
| 21 | R+G | high | Multiple multi-char delimiters `//[**][%%]\n1**2%%3`. |

Roughly 20 commits per language. The gear column is the teaching point: **the same person writes commits 2–8 and commits 19–21, but the stride is not the same**. Low gear bought the design; high gear cashes the bought design.

## Notes per phase

- **Scaffold** (1) — only the project skeleton. No file named after the SUT yet.
- **Red** commits only include the test. Run the test, confirm red, commit.
- **Green** commits write the minimum code to pass the test that was just committed red.
- **Refactor** commits keep the bar green; nothing changes behavior.
- **Reflect** commits are `git commit --allow-empty` with a message explaining the gear decision. They are the written equivalent of a pause at the top of the stairs.
- **Spec (passes on arrival)** commits add a test that is immediately green because the previous triangulation generalized correctly. The commit body says so. This is honest TDD — it shows the moment the algorithm outran the test list.
