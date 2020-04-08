# CharacterCustomizer
#### by Aster
Customize your Survivors by simply changing a value in a config file!

**Important note: a lot of features have been moved to [CharacterCustomizerPlus](https://thunderstore.io/package/AsterAether/CharacterCustomizerPlus/)!**

## Features

* Change almost any vanilla stats of a Survivor and it's skills by changing a
  value in the config file!
* Doesn't overwrite default values if the config value is set to 0,
  improving forward compatibility.
* Multiplayer compatible, players can even have different configs if they want!
* Small vanilla QoL fixes, enabled by default!

## Installation

* Copy the included `CharacterCustomizer.dll` into your BepInEx plugins
  folder.
* Start up the game! This will create the config file. Note that this
  can take a little longer, there are a lot of values to be checked and
  created.

## Configuration

It is highly recommended to use [BepInEx.ConfigurationManager](https://github.com/BepInEx/BepInEx.ConfigurationManager) to edit the configuration values in-game with the F1 key.
**Live update is supported for ALL values! With the ConfigManager you can even change values while in-game.**

The configuration file is located in the config folder of BepInEx, called at.aster.charactercustomizer.cfg. 
It initializes with all values set to their default values. If a value is left with the default one (0 in cases of numbers), 
the executing code in the plugin will be skipped, and vanilla risk of rain behavior will be used.

A sample config line would look like this:
```
## Commando: The base health of your survivor. Vanilla value: 110
# Setting type: Single
# Default value: 0
CommandoBaseMaxHealth = 0
```
The first line is a comment explaining the configuration value, and is automatically updated by the game to include the vanilla RoR2 value of the stat.
The second line is the type of value expected (Single = Decimal).
And the second line is the actual config value, where you can change the stat to your liking.

CharacterCustomizer will try to add the vanilla values as references in
the comments of the config file. If you seem to be missing some values, try playing a run of the game, that should update everything.

Please use dots for separating the decimal values (0.1) and not commas (0,1).


## Available Config Values

**See:**
[Config Values](https://github.com/AsterAether/CharacterCustomizer/blob/master/config_values.md)

## Changelog

**See:**
[Changelog](https://github.com/AsterAether/CharacterCustomizer/blob/master/CHANGELOG.md)