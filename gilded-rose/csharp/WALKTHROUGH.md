# Gilded Rose — C# Walkthrough

Commit-by-commit TDD tour of the C# implementation (.NET 8 • xUnit • FluentAssertions).

Each row is one scenario from [`../SCENARIOS.md`](../SCENARIOS.md),
driven red → green in a single commit. Test names match scenario titles
verbatim, modulo the language's naming convention.

| Step | Commit | Description |
|------|--------|-------------|
| scaffold | [`74939cd`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/74939cd24f1fdc8ca7bdfbafea8ed414ae761339) | scaffold xUnit solution with domain vocabulary |
| scenario 1 | [`e1ecaae`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/e1ecaae402099eb7465523f61711c15f9ac9fcb3) | standard items lose one quality per day while fresh |
| scenario 2 | [`3a6eb84`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/3a6eb846dbccb79e358a8944754b595c58fd51c4) | standard items lose two quality per day after the sell-by date |
| scenario 3 | [`ed86f21`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/ed86f21c400e04648e48027c6e1aa64e392a82fd) | standard item quality never goes below zero |
| scenario 4 | [`b9a1812`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/b9a18127b3c0f1ebad65652bcaf2b5a12272b264) | standard item sell-in decreases by one each day |
| scenario 5 | [`a6775bf`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/a6775bf8f98e2960ccc5aecf3088327e3fd479b8) | aged items gain one quality per day while fresh |
| scenario 6 | [`057de64`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/057de64c25c999dcab244660f6c3828a832d78b6) | aged items gain two quality per day after the sell-by date |
| scenario 7 | [`22f0978`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/22f09782ab6c4f26b28cbcb4d2fb2b8c8e041081) | aged item quality never exceeds fifty |
| scenario 8 | [`c93b8e7`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/c93b8e73e53cba989b638222939981290663f3f4) | aged item sell-in decreases by one each day |
| scenario 9 | [`76a3a8d`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/76a3a8d7cc76207ea2dc7de9d240f6d24eb9b46f) | legendary items never lose quality |
| scenario 10 | [`8aa28db`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/8aa28db11050527cec6c926d3a9569bfbe640ddb) | legendary item sell-in never changes |
| scenario 11 | [`f92f0f4`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/f92f0f4f851bfa9ccd7b9f26db33e1fdad209593) | legendary items may have quality above fifty |
| scenario 12 | [`e989011`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/e9890111d2938fdf922ea60bae50309bacd2c6a0) | backstage pass quality increases by one when concert is more than ten days away |
| scenario 13 | [`f5c24b6`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/f5c24b648bb8fe7fbf9a9e344dcafb77fba1d6af) | backstage pass quality increases by two when concert is ten days or fewer away |
| scenario 14 | [`f224f22`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/f224f22286f4df529151758278c40b3a8de61918) | backstage pass quality increases by three when concert is five days or fewer away |
| scenario 15 | [`cbd730c`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/cbd730c0e5272c25d87f714a76dec7df0464b6ef) | backstage pass quality drops to zero after the concert |
| scenario 16 | [`e364e33`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/e364e33b658999bd1379241040e19c73d6edc033) | backstage pass quality never exceeds fifty before the concert |
| scenario 17 | [`5f18621`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/5f18621b52e5ea15c919a88bd56152caca9053c1) | conjured items lose two quality per day while fresh |
| scenario 18 | [`50c63a9`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/50c63a99f7453e292fd6f784d7f25dc6a04d5eb4) | conjured items lose four quality per day after the sell-by date |
| scenario 19 | [`5f7be42`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/5f7be4274f42402f84f1b121ada401faa1206af4) | conjured item quality never goes below zero |
| scenario 20 | [`4fb8370`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/4fb8370afb403078ba800d3ffe9f12cd565d07ac) | mixed inventory: each item follows its own category rules on the same day |
| scenario 21 | [`aa8b433`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/aa8b433779f664136212db101441c491e63d2d44) | multi-day aging: ten days of updates applied in sequence produce correct quality progression |
| refactor | [`78c962e`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/78c962e36c2773394767b4011cd717bd58eb6e98) | stack ItemBuilder fluent chains for readability |

## How to run

```bash
cd gilded-rose/csharp
dotnet test
```
