# String Calculator — C# Walkthrough

This kata ships in **[Pedagogy mode](../../README.md#1-pedagogy--learn-the-tdd-rhythm)**. The commit log is the teaching. Walk it top to bottom and you feel the rhythm: red → green, red → green, refactor, reflect — and the moment the gear shifts from **low** (fake-it, triangulate one data point at a time) to **middle** (one scenario per cycle) to **high** (write the obvious implementation; the later scenarios pass on arrival).

The gear shifts are the teaching point. Low gear bought the design; high gear cashes it.

| Step | Commit | Cycle | Gear | What was learned |
|------|--------|-------|------|------------------|
| 1 | [`6f56709`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/6f5670996af399f30c6b85460df87c67ffa10418) | scaffold | — | Empty xUnit solution. No SUT yet — the first failing test will name it. |
| 2 | [`281c6cc`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/281c6cccdfe2e723102e01bb9b0acbf930895ec0) | red | low | First test. Compilation fails because `Calculator` doesn't exist. |
| 3 | [`2fb3231`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/2fb3231319ec7c939d42e0c6e94ef4ef8d137479) | green (fake-it) | low | `return 0`. Restraint is the lesson; resist the urge to write `int.Parse`. |
| 4 | [`72822b4`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/72822b4a9f418549047b2591a8fbda1c93a07626) | red | low | Single number — second data point, forces a real parse. |
| 5 | [`687ca99`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/687ca9920e398a7ad67f04dd3d532cdd69a2eadd) | green | low | `int.Parse` with an empty guard. Two data points are still two special cases. |
| 6 | [`f205c3a`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/f205c3a29a570600e921c9dac99557cd0e75e90d) | red | low | Two numbers — the third data point pushes toward a general shape. |
| 7 | [`e0e77f9`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/e0e77f9df54fcef3776d9aaf064dc5ba9e69689e) | green | low | Split on comma, fold. Already handles any count — over-solving is honest here. |
| 8 | [`17c030d`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/17c030db52c8acca9c307e217b03d89c58e63ece) | refactor | low | Swap the imperative fold for `Split(',').Sum(int.Parse)`. |
| 9 | [`f115fcb`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/f115fcb4c69a6e8f96b4ac2b61270a0ebfbe1419) | reflect | low → middle | Empty commit. Triangulation is done; shift gear. |
| 10 | [`db7df99`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/db7df9973aebdbf360b652f7636fa093af1cdf61) | spec (passes on arrival) | middle | Many-numbers test goes green immediately. That's the honest outcome. |
| 11 | [`d52e458`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/d52e458d79fc7909f149826cf73e8a431f85cd54) | red | middle | Newline delimiter. Split on `,` alone fails. |
| 12 | [`aa5b1b1`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/aa5b1b1e47ef6b8645edf2d146426354439ea332) | green | middle | `Split(',', '\n')` — comma and newline are both delimiters. |
| 13 | [`c1e9b10`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/c1e9b109e746376bb29c3fd9c4a971d758d15b63) | reflect | middle | Empty commit. Function shape is settled; continue in middle gear. |
| 14 | [`905844c`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/905844c77f5fa4fb855575e6000e403e28497c41) | red+green | middle | Custom delimiter `//;\n1;2`. Inline header parsing for now. |
| 15 | [`f7d4083`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/f7d4083dd94a75f8933675ad0ac39c44c9b720d5) | refactor | middle | Extract `DelimiterParser`. `Add` now reads as *parse header, split body, sum*. |
| 16 | [`2f7676c`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/2f7676c52949be60b0c2b133d8408fa110ad25e9) | red+green | middle | Negative throws with listing message. `string.Join(", ", negatives)` already handles multi. |
| 17 | [`c98cfcf`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/c98cfcf780b63e2b4c31d40130f91c8448bd536c) | spec (passes on arrival) | middle | Multi-negative. Honest pass — the previous `Join` generalized correctly. |
| 18 | [`b782085`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/b78208573ab78b93784f9b14843aeeded4405ff0) | red+green | middle | Numbers > 1000 are ignored. `Where(n => n <= 1000)` before `Sum()`. |
| 19 | [`412dd45`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/412dd4564307dbb1ab7f0561665585f0dd73a4b3) | red+green | high | Any-length bracketed delimiter. Regex in the parser opens the door for N groups. |
| 20 | [`131af29`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/131af29624fef1de334b49c576f0821f615cefdc) | spec (pass on arrival) | high | Multiple and multi-char delimiters — both immediate passes. |

## How to run

```bash
cd string-calculator/csharp
dotnet test
```

## The takeaway

Two reflect commits. One refactor-extraction commit. Three spec-on-arrival commits where the log tells you explicitly *no new code was needed*. That honesty is the difference between a teaching log and a theater log. It says: here is where the design paid off, and here is where it paid off again.
