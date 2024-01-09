### 2.1.2
- LethalAPI_Terminal fixed an [issue](https://github.com/LethalCompany/LethalAPI.Terminal/issues/30). Needed to bump version

### 2.1.1
- LethalConfig fixed an [issue](https://github.com/AinaVT/LethalConfig/issues/19) from their end. Needed to bump version

### 2.1.0
- Added support for [LethalConfig](https://thunderstore.io/c/lethal-company/p/AinaVT/LethalConfig/)
- Added support for [LethalAPI.Terminal](https://thunderstore.io/c/lethal-company/p/LethalAPI/LethalAPI_Terminal/)
- Added support for [ProgressiveDeadline](https://thunderstore.io/c/lethal-company/p/LethalOrg/ProgressiveDeadline/)
- Added config menu to give the host the option for each save file to choose if he prefers correlated price or constant value price (350 credits) for each extra day

### 2.0.6
- Fixed version in DLL file, required new version bump

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

### 2.0.1
- Making this mod working with [DynamicDeadline](https://thunderstore.io/c/lethal-company/p/Krayken/DynamicDeadline/) so you can have dynamic deadlines, but if needed you can purchase more days

### 2.0.0
- Refactoring all code base and adjusting dependencies

### 1.0.2
- Fixing when you could buy an extra day but you got the wrong terminal response message ([#2](https://github.com/ustaalon/LethalCompany.ExtraDays/issues/2))
- Fixing when you could still see your extra day after finishing cycle (going to company) or after death ([#3](https://github.com/ustaalon/LethalCompany.ExtraDays/issues/3))

### 1.0.1
- Added more information about the mod
- Adjusting price logic to be more reachable (not too expensive or way too low)

### 1.0.0
- Basic logic and terminal improvements