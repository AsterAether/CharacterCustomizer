# Changelog

* **v2.0.0:** - Big rewrite and update to newest RoR2 version.
* **v1.0.2:** - Naming changes of config values, to the real names of skills used in the game.
* **v1.0.1:** - Rewrote internal structure (again), and updated everything to new Artifact version of RoR2
* **v1.0.0:** - Rewrote internal structure to deprecate AetherLib, updated codebase to allow multiple skills per slot per survivor. Moved extra changes to new project [CharacterCustomizerPlus](https://github.com/AsterAether/CharacterCustomizerPlus).
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