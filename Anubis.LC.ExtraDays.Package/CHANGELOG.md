### 2.0.5
- Fixing enemies not spawning (also in DynamicDeadline) when you have higher deadline (edge case missed)

### 2.0.4
- Fixing enemies not spawning (also in DynamicDeadline) when you have higher deadline (more than 10 days) ([#20](https://github.com/ustaalon/LethalCompany.ExtraDays/issues/20))

### 2.0.3
- Fixing buying rate sync calculation
- Fixing group credits sync after buying an extra day ([#18](https://github.com/ustaalon/LethalCompany.ExtraDays/issues/18))
- Fixing when buying an extra day not sync with everybody else ([#19](https://github.com/ustaalon/LethalCompany.ExtraDays/issues/19))
    - Only ship's captain (host) can purchase an extra day (with that everyone will be in sync)

### 2.0.2
- Fixing buying rate issues due to high deadline days ([#10](https://github.com/ustaalon/LethalCompany.ExtraDays/issues/10))
- Fixing terminal issues with the `buyday` command ([#9](https://github.com/ustaalon/LethalCompany.ExtraDays/issues/9))
- Fixing minor bugs and refactoring
- Fixing buying rate reset on new loaded game (loading saved file) or negative buying rate

[All changelog can be found here](https://github.com/ustaalon/LethalCompany.ExtraDays/blob/rc/CHANGELOG.md)