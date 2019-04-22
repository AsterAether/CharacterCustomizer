# CharacterCustomizer [![Build Status](https://travis-ci.com/AsterAether/CharacterCustomizer.svg?branch=master)](https://travis-ci.com/AsterAether/CharacterCustomizer)
#### by Aster
Customize your Survivors by simply changing a value in a config file!

## Features

* Change almost any vanilla stats and quirks of a Survivor by changing a
  value in the config file
* Doesn't overwrite default values if the config value is set to 0,
  improving forward compatibility
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

The configuration file is located in the config folder of BepInEx,
called `at.aster.CharacterCustomizer.cfg`. It initializes with all
values set to their default values. If a value is left with the default
one (0 in cases of numbers), the executing code in the plugin will be
skipped, and vanilla risk of rain behavior will be used.

CharacterCustomizer will try to add the Vanilla values as references in
the comments of the config file. If you seem to be missing some values,
for instance the Vanilla values of the Mercenary Dash skill, try loading
into the game and dashing with him. This should update the config file
with the real values.


## Available Config Values

**See:** 

## Changelog

* **v0.0.5** - 
* **v0.0.4** - Fixed up Artificer and Engineer IL patches to be more
  efficient, and documentation pass.
* **v0.0.3** - Added Mercenary and Huntress back to be configured.