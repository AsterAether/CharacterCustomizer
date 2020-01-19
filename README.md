# CharacterCustomizer [![Build Status](https://travis-ci.com/AsterAether/CharacterCustomizer.svg?branch=master)](https://travis-ci.com/AsterAether/CharacterCustomizer)
#### by Aster
Customize your Survivors by simply changing a value in a config file!

## Features

* Change almost any vanilla stats and quirks of a Survivor by changing a
  value in the config file
* Doesn't overwrite default values if the config value is set to 0,
  improving forward compatibility
* Multiplayer compatible, players can even have different configs if they want!
* Small vanilla QoL fixes, enabled by default!
* Extra features that can be enabled, that equip the Survivors with new
  behaviours!

## Installation

* Install
  [AetherLib](https://thunderstore.io/package/AsterAether/AetherLib/)
  first.
* Copy the included `CharacterCustomizer.dll` into your BepInEx plugins
  folder.
* Start up the game! This will create the config file. Note that this
  can take a little longer, there are a lot of values to be checked and
  created.

## Configuration

It is highly recommended to use [BepInEx.ConfigurationManager](https://github.com/BepInEx/BepInEx.ConfigurationManager) to edit the configuration values in-game with the F1 key.
**Live update is supported for basic values in skills and survivor body (Cooldown of skills or MaxHealth for example). You will need to restart your run though for the changes to take effect.**
**You will need to restart the game after changing other values that affect gameplay though.**

The configuration file is located in the config folder of BepInEx, called at.aster.CharacterCustomizer.cfg. 
It initializes with all values set to their default values. If a value is left with the default one (0 in cases of numbers), 
the executing code in the plugin will be skipped, and vanilla risk of rain behavior will be used.

A sample config line would look like this:
```
# Commando: The base health of your survivor. Vanilla value: 110
CommandoBaseMaxHealth = 200
```
The first line is a comment explaining the configuration value, and is automatically updated by the game to include the vanilla RoR2 value of the stat.
And the second line is the actual config value, where you can change the stat to your liking.

CharacterCustomizer will try to add the Vanilla values as references in
the comments of the config file. If you seem to be missing some values,
for instance the Vanilla values of the Mercenary Dash skill, try loading
into the game and dashing with him. This should update the config file
with the real values.

Please use dots for separating the decimal values (0.1) and not commas (0,1).


## Available Config Values

**See:**
[Config Values](https://github.com/AsterAether/CharacterCustomizer/blob/master/config_values.md)

## Changelog

* **v0.3.7:** - Now with live update of config values when using [BepInEx.ConfigurationManager](https://github.com/BepInEx/BepInEx.ConfigurationManager)! And Commando Barrage scaling fix.
* **v0.3.6:** - Readme update and new RoR2 version with good body doggo customization.
* **v0.3.4:** - Fixed Loader skill names in config.
* **v0.3.3:** - Readme update.
* **v0.3.2:** - Reverted multiple file change to be compatible with BepInEx.ConfigurationManager.
* **v0.3.1:** - (NOT TESTED) Moved configuration to it's own folder and own file per survivor. Added missing config values, still experimental and not everything is tested!
* **v0.3.0:** - (TEMPORARY UPDATE) Updated to newest BepInEx and RoR2 version. Only basic values for now, other settings coming as I fix them. Please ping me in the discord for anything not working as expected!
* **v0.2.12** - Updated README to include explanation of config values.
* **v0.2.11** - Updated dependency versions.
* **v0.2.10** - Update to new AetherLib version.
* **v0.2.9** - Updated to new game version, and added new survivor.
* **v0.2.7** - Changed loading times of stat changes from start of Run to start of Application.
* **v0.2.6** - Fixed artificer Firebolt stock scaling, and Flamethrower tick scaling (Why scaling oh why).
* **v0.2.5** - Fixed artificer Firebolt cooldown scaling.
* **v0.2.4** - Added optional time limit to invulnerability to Commandos role, and fixed a bug with scaling Barrage.
* **v0.2.3** - Added option for Commandos role to grant him invulnerability frames while rolling.
* **v0.2.2** - Fixed up scaling mistake from before, most formulas are now: Stat + Coeff * (ATKSP - 1) * Stat. Also added Artificer Flamethrower downscale with attack speed.
* **v0.2.1** - Changed attack speed scaling formula to only start affecting the scaling stat with additional attack speed. E.G.: Formula now isn't Coeff * ATKSP * Stat, it's Coeff * (ATKSP - 1) * Stat
* **v0.2.0** - Added base character body values for every survivor, and rewrote the baseline code to be more manageable.
* **v0.1.1** - Added Artificer Firebolt scaling and Flamethrower scaling options
* **v0.1.0** - Added cooldown and stock manipulation config values to
  every character
* **v0.0.4** - Fixed up Artificer and Engineer IL patches to be more
  efficient, and documentation pass.
* **v0.0.3** - Added Mercenary and Huntress back to be configured.