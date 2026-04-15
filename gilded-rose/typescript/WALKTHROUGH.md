# Gilded Rose — TypeScript Walkthrough

Commit-by-commit TDD tour of the TypeScript implementation (Node 20 • Vitest • strict TypeScript).

Each row is one scenario from [`../SCENARIOS.md`](../SCENARIOS.md),
driven red → green in a single commit. Test names match scenario titles
verbatim, modulo the language's naming convention.

| Step | Commit | Description |
|------|--------|-------------|
| scaffold | [`83f5c38`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/83f5c38cbbc00e9053055e98343be6e68918935e) | scaffold vitest project with domain vocabulary |
| scenario 1 | [`fab6d09`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/fab6d09aa250499072f5ef1742a710d0f2246680) | standard items lose one quality per day while fresh |
| scenario 2 | [`8cf5f7d`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/8cf5f7d8ba7daa2c6242634e3118743c673d91ab) | standard items lose two quality per day after the sell-by date |
| scenario 3 | [`d215e96`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/d215e96edb566d54a47beff57211dde1138ac1eb) | standard item quality never goes below zero |
| scenario 4 | [`4983271`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/4983271f4d7237edc5ed61ae60f9f64afa96494b) | standard item sell-in decreases by one each day |
| scenario 5 | [`31ee086`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/31ee0861fb0d5e04d80230c75abcc370ade8120b) | aged items gain one quality per day while fresh |
| scenario 6 | [`0ee1380`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/0ee13807130c5301b83f65862253f6308297801b) | aged items gain two quality per day after the sell-by date |
| scenario 7 | [`8189512`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/8189512c5d3ded61a7076564de80c6b324962d15) | aged item quality never exceeds fifty |
| scenario 8 | [`8b7b958`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/8b7b958d10a28ee273adc94e9d97e3a26c29cc0b) | aged item sell-in decreases by one each day |
| scenario 9 | [`469a366`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/469a3666e4770664f89a9cc291d0e2f5a00d8c95) | legendary items never lose quality |
| scenario 10 | [`63b9c3b`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/63b9c3b11d8e506665bab0bec3e242fc532327b2) | legendary item sell-in never changes |
| scenario 11 | [`a294f3a`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/a294f3a676a90b2640b300a7b5333fb242ca1715) | legendary items may have quality above fifty |
| scenario 12 | [`4476410`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/44764106c9356bf1905f866b9c5f800d96c469fb) | backstage pass quality increases by one when concert is more than ten days away |
| scenario 13 | [`620b804`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/620b8045985e236d04fb2fed794841d81a7d972f) | backstage pass quality increases by two when concert is ten days or fewer away |
| scenario 14 | [`474438f`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/474438f45506e4831ea96af330ac1fd375ed85b7) | backstage pass quality increases by three when concert is five days or fewer away |
| scenario 15 | [`d70a113`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/d70a11361a4934f601649f99df625bc2ab6b0e1d) | backstage pass quality drops to zero after the concert |
| scenario 16 | [`02f5a9a`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/02f5a9a9374bca729b8f43ccbb77736147fa8e0c) | backstage pass quality never exceeds fifty before the concert |
| scenario 17 | [`89f1c6f`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/89f1c6f35525a66bb869fd00d1e2f19d66a447a0) | conjured items lose two quality per day while fresh |
| scenario 18 | [`a9ae3ba`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/a9ae3babba85a206651d3f6e2f80029dbf1b3f33) | conjured items lose four quality per day after the sell-by date |
| scenario 19 | [`3cce0e7`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/3cce0e7db3d60df6b0bd258d2dc8e648e867201c) | conjured item quality never goes below zero |
| scenario 20 | [`33be5a7`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/33be5a7022cd902fabde14e810e0b83c4665e63e) | mixed inventory: each item follows its own category rules on the same day |
| scenario 21 | [`255ce89`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/255ce890fe53a5e66551ad9f5c4fa2c809ef87ff) | multi-day aging: ten days of updates applied in sequence produce correct quality progression |
| refactor | [`b3506ad`](https://github.com/Kode-Rex/tddbuddy-reference-katas/commit/b3506ad7e211168595665468d5ffb50e363e36f2) | stack ItemBuilder fluent chains for readability |

## How to run

```bash
cd gilded-rose/typescript
npm install
npm test
```
