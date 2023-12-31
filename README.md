# Extra Days
Allows the player to purchase an extra day via the terminal. This mod's uniqueness is that it tries to be more realistic with the game, as it is not trying to modify the main logic of the deadline. It tries to add functionality that will give you extends for the deadline but with some price on it. **You can install it in additon to Dynamic Deadline mod!**

## How To Use?
1. Enter the Ship's Terminal
2. Go to the `Other` category
3. Execute the `buyday` menu item
4. CONFIRM / DENY purchase of an extra day

All players need to have the mod installed to play.

## Bugs & Issues
Please let me know about any bugs or issues with my mod by opening an [issue](https://github.com/ustaalon/LethalCompany.ExtraDays/issues) or by sending me an email at bowbeforeanubis@gmail.com

# Release Notes
### 2.0.2
- Fixing buying rate issues due to high deadline days ([issue 10](https://github.com/ustaalon/LethalCompany.ExtraDays/issues/10))
- Fixing terminal issues with the `buyday` command ([issue 9](https://github.com/ustaalon/LethalCompany.ExtraDays/issues/9))
- Buying rate will now be saved to local (for host only)
- Fixing minor bugs

### 2.0.1
- Making the mod working with [DynamicDeadline](https://thunderstore.io/c/lethal-company/p/Krayken/DynamicDeadline/) so you can have dynamic deadlines but if needed - you can purchase more days

### 2.0.0
- Refactoring all code base and adjusting dependencies

### 1.0.2
- Fixing when you could buy an extra day but you got the wrong terminal response message ([issue #2](https://github.com/ustaalon/LethalCompany.ExtraDays/issues/2))
- Fixing when you could still see your extra day after finishing cycle (going to company) or after death ([issue #3](https://github.com/ustaalon/LethalCompany.ExtraDays/issues/3))

### 1.0.1
- More information about the mod
- Adjusting price logic to be more reachable (not too expensive or way too low)

### 1.0.0
- Basic logic and terminal improvements

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

## Notes
This is my first mod. I had some free time during the holidays and decided to give it a try.
