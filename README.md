# Extra Days
Allows the player to purchase an extra day via the terminal. This mod's uniqueness is that it tries to be more realistic with the game, as it is not trying to modify the main logic of the deadline. It tries to add functionality that will give you extends for the deadline but with some price on it. You can buy many days as you want, if you have enough credits for it. Every cycle (all days passed) game deadline will reset to defaults - 3 days.

## How To Use? (only host can do it)
1. Enter the ship's terminal
2. Go to the `Other` category
3. Execute the `buyday` menu item
4. CONFIRM / DENY purchase of an extra day

All players need to have the mod installed to play.

### If you are using it with DynamicDeadline mod
I did the relevant adjustments so it will work without any issues, so you can play both mods together. Moreover, I fix two major issues that happens when you have higher deadline:
- Enemies are not spawning when you have deadline higher than 10 days or more
- Negative buying rate from the company
So by using my mod, it will fix those issues for both of them.

## Bugs, Issues & requests
Please let me know about any bugs, issues or requests regarding my mod by opening an [issue](https://github.com/ustaalon/LethalCompany.ExtraDays/issues) or by sending me an email at bowbeforeanubis@gmail.com

# Release Notes
### 2.0.3
- Fixing buying rate sync calculation
- Fixing group credits sync after buying an extra day ([#18](https://github.com/ustaalon/LethalCompany.ExtraDays/issues/18))
- Fixing when buying an extra day not sync with everybody else ([#19](https://github.com/ustaalon/LethalCompany.ExtraDays/issues/19))
    - Only ship's captain (host) can purchase an extra day (with that everyone will be in sync)

### 2.0.2
- Fixing buying rate issues due to high deadline days ([#10](https://github.com/ustaalon/LethalCompany.ExtraDays/issues/10))
- Fixing terminal issues with the `buyday` command ([#9](https://github.com/ustaalon/LethalCompany.ExtraDays/issues/9))
- Fixing minor bugs and refactoring
- Buying rate will now be saved to local (for host only)

### 2.0.1
- Making the mod working with [DynamicDeadline](https://thunderstore.io/c/lethal-company/p/Krayken/DynamicDeadline/) so you can have dynamic deadlines but if needed - you can purchase more days

### 2.0.0
- Refactoring all code base and adjusting dependencies

[All changelog can be found here](https://github.com/ustaalon/LethalCompany.ExtraDays/blob/rc/Anubis.LC.ExtraDays.Package/CHANGELOG.md)

# Additional Credits
Thanks to [Lethal Company Community on Github](https://github.com/LethalCompany) for making modding easy for Lethal Company.
Thanks, also, to [LethalAPI.TerminalCommands](https://github.com/LethalCompany/LethalAPI.TerminalCommands) for adding the ability to make new commands.

By using that mod, these changes are apply as well:
* Reduces the delay after entering the terminal before you can type by 80%
  *  You ever open the terminal and start typing, even hearing the keyboard sound, but it doesn't write anything? This fixes that.

* Disable scrolling to the top of the terminal on every command execution
  *  This now only happens when a command clears the screen.
    
* Trims newlines from the start of command responses
  *  The game force adds newlines to the start of commands, which causes issues for commands that don't clear the terminal. 

* `Terminal.PlayVideoFile(string filePath)`
  * Allows you to play local files in the background of the terminal
    
* `Terminal.PlayVideoLink(Uri url)`
  * Allows you to play remote videos in the background of the terminal