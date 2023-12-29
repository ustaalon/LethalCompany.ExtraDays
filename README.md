# Extra Days
Allows the player to purchase an extra day via the terminal. This mod's uniqueness is that it tries to be more realistic with the game, as it is not trying to modify the main logic of the deadline. It tries to add functionality that will give you extends for the deadline but with some price on it.

## How To Use?
1. Enter terminal
2. Go to 'Other' menu item
3. Go to 'Buydays' menu item
4. CONFIRM / DENY purchase of an extra day

## Bugs & Issues
Please let me know of any bugs or issues happening to you regarding using my mod, by sending me an email: bowbeforeanubis@gmail.com or open an issue in the GitHub repo

# Release Notes
## 2.0.0
- Refactoring all code base and adjusting dependecies

## 1.0.2
- Fixing [issue #2](https://github.com/ustaalon/LethalCompany.ExtraDays/issues/2) when you could buy an extra day but you got the wrong terminal response message
- Fixing [issue #3](https://github.com/ustaalon/LethalCompany.ExtraDays/issues/3) when you could still see your extra day after finishing cycle (going to company) or after death

## 1.0.1
- More information about the mod
- Adjusting price logic to be more reachable (not too expensive or way too low)

## 1.0.0
- Basic logic and terminal improvements

## Additional Credits
Thanks to [Lethal Company Community on Github](https://github.com/LethalCompany) for making this template code base. 

### Important change in the future
After gaining more understanding, the codebase that used for the custom terminal commands will be refactor to be used as DLL and will be remove from this repo, as it based on [this repo mode](https://github.com/LethalCompany/LethalAPI.TerminalCommands),
that added those adjustments and fixes to the terminal: 
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