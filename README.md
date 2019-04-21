# CharacterCustomizer [![Build Status](https://travis-ci.com/AsterAether/CharacterCustomizer.svg?branch=master)](https://travis-ci.com/AsterAether/CharacterCustomizer)
#### by Aster
Customize your Survivors by simply changing a value in a config file!

## Features

* Change almost any vanilla stats and quirks of a Survivor by changing a
  value in the config file
* Doesn't overwrite default values if the config value is set to 0,
  improving forward compatibility
* Extra features that can be enabled, that equip the Survivors with new
  behaviours!

## Installation

* Install
  [AetherLib](https://thunderstore.io/package/AsterAether/AetherLib/)
  first.
* Copy the included `CharacterCustomizer.dll` into your BepInEx plugins
  folder.

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

### Engineer
* **GrenadeMaxChargeTime:** Maximum charge time (animation) for grenades, in seconds.
* **GrenadeMaxFireAmount:** The maximum number of grenades the Engineer can fire.
* **GrenadeMinFireAmount:** The minimum number of grenades the Engineer fires.
* **GrenadeSetChargeCountToFireAmount:** Set the steps when charging (animation) to the same amount as FireAmount.
* **GrenadeTotalChargeDuration:** Maximum charge duration (logic) for grenades, in seconds.
* **MineCooldown:** Cooldown of the Mine skill, in seconds
* **MineMaxDeployCount:** The maximum number of mines the Engineer can place.
* **ShieldCooldown:** Cooldown of the Shield skill, in seconds
* **ShieldDuration:** The number of seconds the shield is active.
* **ShieldEndlessDuration:** If the duration of the shield should be endless.
* **ShieldMaxDeployCount:** The maximum number of shields the Engineer can place.
* **TurretCooldown:** Cooldown of the Turret skill, in seconds
* **TurretMaxDeployCount:** The maximum number of turrets the Engineer can place.
### Commando
* **BarrageBaseDurationBetweenShots:** Base duration between shots in the Barrage skill.
* **BarrageBaseShotAmount:** How many shots the Barrage skill should fire
* **BarrageCooldown:** Cooldown of the Barrage Skill, in seconds
* **BarrageScaleModifier:** Modifier for the attackspeed scale, in percent.
* **BarrageScalesWithAttackSpeed:** If the barrage bullet count should scale with attackspeed. Idea by @Twyla. Needs BarrageScaleModifier to be set.
* **DashCooldown:** Cooldown of the dash, in seconds
* **DashResetsSecondCooldown:** If the dash should reset the cooldown of the second ability.
* **DashStockCount:** How many stocks the dash ability has.
* **LaserCooldown:** Cooldown of the secondary laser, in seconds
* **LaserDamageCoefficient:** Damage coefficient for the secondary laser in percent.
* **PistolBaseDuration:** Base duration for the pistol shot in percent. (Attack Speed)
* **PistolDamageCoefficient:** Damage coefficient for the pistol in percent.
* **PistolHitLowerBarrageCooldown:** If the pistol hit should lower the Barrage Skill cooldown.
* **PistolHitLowerBarrageCooldownPercent:** The amount in percent of the current maximum cooldown the Barrage Skill should be lowered by. Needs to have PistolHitLowerBarrageCooldownPercent set.
### Artificer
* **FireboltAttackSpeedCooldownScaling:** If the cooldown of the Firebolt Skill should scale with AttackSpeed.
* **FireboltAttackSpeedCooldownScalingCoefficent:** Coefficient for cooldown AttackSpeed scaling, in percent.
* **FireboltAttackSpeedStockScaling:** If the charge count of the FireBolt Skill should scale with AttackSpeed.
* **FireboltAttackSpeedStockScalingCoefficent:** Coeefiecent for charge AttackSpeed scaling, in percent.
* **FireBoltChargeCount:** Charge count of the Firebolt skill.
* **FireboltCooldown:** Cooldown of the Firebolt skill, in seconds
### MultT
* **NailgunSpreadPitch:** Pitch spread of the nailgun, in percent
* **NailgunSpreadYaw:** Yaw spread of the nailgun, in percent


## Changelog

* v0.0.3 - Added Mercenary and Huntress back to be configured.