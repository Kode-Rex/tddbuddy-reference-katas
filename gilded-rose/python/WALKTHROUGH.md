# Gilded Rose — Python Walkthrough

Commit-by-commit TDD tour of the Python implementation (Python 3.11 • pytest • dataclasses).

Each row is one scenario from [`../SCENARIOS.md`](../SCENARIOS.md),
driven red → green in a single commit. Test names match scenario titles
verbatim, modulo the language's naming convention.

| Step | Commit | Description |
|------|--------|-------------|
| scaffold | [`7dd2576`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/7dd25765de603e629c87f24b573b85b96757f7c5) | scaffold pytest project with domain vocabulary |
| scenario 1 | [`684211c`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/684211c021f6ff1431ab2db5967fe922419916a8) | standard items lose one quality per day while fresh |
| scenario 2 | [`067cdb8`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/067cdb8623154a87763f9a497d5e34d517a2dc88) | standard items lose two quality per day after the sell-by date |
| scenario 3 | [`213d45f`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/213d45fbc7c9b2a7e2b59061d7bc7f59c7695362) | standard item quality never goes below zero |
| scenario 4 | [`a952029`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/a952029da1af0ad322e12612568ff9ccdde9d6bf) | standard item sell-in decreases by one each day |
| scenario 5 | [`14c41a4`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/14c41a4e10bc790312cd516edf88b3f7f6376171) | aged items gain one quality per day while fresh |
| scenario 6 | [`e418745`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/e418745bb0286f937d7e172a7274bc634cb95393) | aged items gain two quality per day after the sell-by date |
| scenario 7 | [`f9712cb`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/f9712cb45287dc7161b79434743360bef59ac7cc) | aged item quality never exceeds fifty |
| scenario 8 | [`12e5092`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/12e5092cd073b046e178413751d5d1614d7c6cb3) | aged item sell-in decreases by one each day |
| scenario 9 | [`7bcebd1`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/7bcebd15c4ab24abf8a8ecfb2a686c550d5cdbca) | legendary items never lose quality |
| scenario 10 | [`277d6d3`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/277d6d328629e35b76cf789fc6b7e1f81cd4eabc) | legendary item sell-in never changes |
| scenario 11 | [`bddabd4`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/bddabd459d283e9cfad32e4b8060de5621a884b9) | legendary items may have quality above fifty |
| scenario 12 | [`b662ec4`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/b662ec453cf688482363b345c41f5c5a4cdee8fd) | backstage pass quality increases by one when concert is more than ten days away |
| scenario 13 | [`14501e3`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/14501e343f36a5ede33d40643ba7819c8c13a884) | backstage pass quality increases by two when concert is ten days or fewer away |
| scenario 14 | [`2b761fc`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/2b761fc0942754a9588a6620552a79674cd6174a) | backstage pass quality increases by three when concert is five days or fewer away |
| scenario 15 | [`6c70dc9`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/6c70dc9d9c29498c29868c9537ce1b580ff0170a) | backstage pass quality drops to zero after the concert |
| scenario 16 | [`1a7bd8f`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/1a7bd8ffa46e1fed49b9a4d505b50ed4570a65c1) | backstage pass quality never exceeds fifty before the concert |
| scenario 17 | [`cd5664c`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/cd5664c5b36261297d54eeb5151ead7ad6fc4f9c) | conjured items lose two quality per day while fresh |
| scenario 18 | [`b76259a`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/b76259a48bf5c964d1b244512ee7e6c954701fbb) | conjured items lose four quality per day after the sell-by date |
| scenario 19 | [`8ab1eee`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/8ab1eee9d637b507e5c9c28c81a3f0b2fd027e06) | conjured item quality never goes below zero |
| scenario 20 | [`dbeeada`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/dbeeada013dcdf3e0a83b5ba646f919a084d5812) | mixed inventory: each item follows its own category rules on the same day |
| scenario 21 | [`8b7f271`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/8b7f2716dcdee32054e19ba20f2841fca74d2a67) | multi-day aging: ten days of updates applied in sequence produce correct quality progression |
| refactor | [`0648289`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/064828972259061eff12350190c7c4d843fe0763) | stack ItemBuilder fluent chains for readability |

## How to run

```bash
cd gilded-rose/python
python3 -m venv .venv
.venv/bin/pip install -e '.[dev]'
.venv/bin/pytest
```
