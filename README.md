# Extra Days
Allows the player to purchase an extra day via the terminal. This mod's uniqueness is that it tries to be more realistic with the game, as it is not trying to modify the main logic of the deadline. It tries to add functionality that will give you extends for the deadline but with some price on it. You can buy many days as you want, if you have enough credits for it. Every cycle (all days passed) game deadline will reset to defaults - 3 days.

## How To Use? (only host can do it)
1. Enter the ship's terminal
2. Go to the `Other` category
3. Execute the `buyday` menu item
4. CONFIRM / DENY purchase of an extra day

All players need to have the mod installed to play.

### If you are using it with DynamicDeadline or ProgressiveDeadline mod
I did the relevant adjustments so it will work without any issues, so you can play both mods together. Moreover, I fix two major issues that happens when you have higher deadline:
- Enemies not spawning when you have deadline higher than 10 days or more
- Negative buying rate from the company

So by using my mod, it will fix those issues for both of them.

## Bugs, Issues & requests
Please let me know about any bugs, issues or requests regarding my mod by opening an [issue](https://github.com/ustaalon/LethalCompany.ExtraDays/issues), by leaving a comment in [this Discord channel](https://discord.com/channels/1168655651455639582/1190842600534573056) or by sending me an email at ustaalon@gmail.com

# Release Notes
### 2.1.0
- Added support for [LethalConfig](https://thunderstore.io/c/lethal-company/p/AinaVT/LethalConfig/)
- Added support for [LethalAPI.Terminal](https://thunderstore.io/c/lethal-company/p/LethalAPI/LethalAPI_Terminal/). V1 is finally out!
- Added config menu to give the host option for each save file to choose if he prefer correlated price or constant value price (350 credits) for each extra day
- Added support for [ProgressiveDeadline](https://thunderstore.io/c/lethal-company/p/LethalOrg/ProgressiveDeadline/)

### 2.0.6
- Fixed version in DLL file, required new version bump

### 2.0.5
- Fixing enemies not spawning (also in DynamicDeadline) when you have higher deadline (edge case missed)

### 2.0.4
- Fixing enemies not spawning (also in DynamicDeadline) when you have higher deadline (more than 10 days) ([#20](https://github.com/ustaalon/LethalCompany.ExtraDays/issues/20))

[All changelog can be found here](https://github.com/ustaalon/LethalCompany.ExtraDays/blob/rc/CHANGELOG.md)

## Additional Credits
Thanks to [Lethal Company Community on Github](https://github.com/LethalCompany) for making modding easy for Lethal Company.
Thanks, also, to [LethalAPI.TerminalCommands](https://github.com/LethalCompany/LethalAPI.TerminalCommands) for adding the ability to make new commands.