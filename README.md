# CharacterCustomizer
#### by Aster
Customize your Survivors by simply changing a value in a config file!

## Features

* Change almost any vanilla stats of a Survivor and it's skills by changing a
  value in the config file!
* Doesn't overwrite default values if the config value is set to 0,
  improving forward compatibility.
* Should be compatible with modded survivors and skills!
  
## Multiplayer
Not tested, but seems to work!

## Configuration

It is highly recommended to use [BepInEx.ConfigurationManager](https://github.com/BepInEx/BepInEx.ConfigurationManager) to edit the configuration values in-game with the F1 key.
Or something like r2modmans config editor.

To generate the config options for a survivor, you need to set the "Enabled" option to true for this character and restart the game.

The configuration file is located in the config folder of BepInEx, called Aster.CharacterCustomizer. 
It initializes with all values set to their default values. If a value is left with the default one (0 in cases of numbers), 
the executing code in the plugin will be skipped, and vanilla risk of rain behavior will be used.

A sample config line would look like this:
```
[Captain]

## If changes for this character are enabled. Set to true to generate options on next startup!
# Setting type: Boolean
# Default value: false
Captain Enabled = false
```
The first line is a comment explaining the configuration value, and is automatically updated by the game to include the vanilla RoR2 value of the stat.
The second line is the type of value expected (Single = Decimal).
And the second line is the actual config value, where you can change the stat to your liking.

CharacterCustomizer will try to add the vanilla values as references in
the comments of the config file. If you seem to be missing some values, try playing a run of the game, that should update everything.

Please use dots for separating the decimal values (0.1) and not commas (0,1).

## TODO

* Reimplement live-update
* Check multiplayer compatability
* **Update [CharacterCustomizerPlus](https://thunderstore.io/package/AsterAether/CharacterCustomizerPlus/)!**


## Changelog

**See:**
[Changelog](https://github.com/AsterAether/CharacterCustomizer/blob/master/CHANGELOG.md)