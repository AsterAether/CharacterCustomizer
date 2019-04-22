using System;
using System.Linq;
using System.Reflection;
using AetherLib.Util;
using AetherLib.Util.Config;
using AetherLib.Util.Reflection;
using BepInEx;
using BepInEx.Configuration;
using EntityStates;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using RoR2;
using RoR2.UI;
using UnityEngine;

namespace CharacterCustomizer.CustomSurvivors
{
    namespace Artificer
    {
        public class CustomArtificer : CustomSurvivor
        {
            public ConfigWrapper<bool> FireboltAttackSpeedStockScaling;

            public ValueConfigWrapper<string> FireboltAttackSpeedStockScalingCoefficient;

            public ConfigWrapper<bool> FireboltAttackSpeedCooldownScaling;

            public ValueConfigWrapper<string> FireboltAttackSpeedCooldownScalingCoefficient;

            public ValueConfigWrapper<string> NovaBombMaxChargeDuration;

            public ValueConfigWrapper<string> NovaBombMaxDamageCoefficient;

            public ValueConfigWrapper<string> FlamethrowerDuration;

            public ValueConfigWrapper<string> FlamethrowerTickFrequency;

            public ConfigWrapper<bool> FlamethrowerTickFrequencyScaleWithAttackSpeed;

            public float VanillaTickFrequency;

            public ValueConfigWrapper<string> FlamethrowerTickFrequencyScaleCoefficient;

            public ValueConfigWrapper<string> FlamethrowerProcCoefficientPerTick;

            public ValueConfigWrapper<string> FlamethrowerMaxDistance;

            public ValueConfigWrapper<string> FlamethrowerRadius;

            public ValueConfigWrapper<string> FlamethrowerTotalDamageCoefficient;

            public ValueConfigWrapper<string> FlamethrowerIgnitePercentChance;

            public CustomArtificer() : base(SurvivorIndex.Mage, "Artificer",
                "FireFirebolt",
                "NovaBomb",
                "Wall",
                "Flamethrower")
            {
            }

            public override void InitConfigValues()
            {
                // Firebolt
                FireboltAttackSpeedStockScaling =
                    WrapConfigBool("FireboltAttackSpeedStockScaling",
                        "If the charge count of the FireBolt Skill should scale with AttackSpeed. Needs to have FireboltAttackSpeedStockScalingCoefficent set to work.");


                FireboltAttackSpeedStockScalingCoefficient =
                    WrapConfigFloat("FireboltAttackSpeedStockScalingCoefficient",
                        "Coefficient for charge AttackSpeed scaling, in percent. Formula: Stock * ATKSP * Coeff.");

                FireboltAttackSpeedCooldownScaling = WrapConfigBool("FireboltAttackSpeedCooldownScaling",
                    "If the cooldown of the Firebolt Skill should scale with AttackSpeed. Needs to have FireboltAttackSpeedCooldownScalingCoefficent set to work.");


                FireboltAttackSpeedCooldownScalingCoefficient = WrapConfigFloat(
                    "FireboltAttackSpeedCooldownScalingCoefficient",
                    "Coefficient for cooldown AttackSpeed scaling, in percent. Formula: BaseCooldown * (1 / (ATKSP * Coeff)).");

                // NovaBomb
                NovaBombMaxChargeDuration = WrapConfigFloat("NovaBombBaseChargeDuration",
                    "Base max charging duration of the NovaBomb");

                NovaBombMaxDamageCoefficient = WrapConfigFloat("NovaBombMaxDamageCoefficient",
                    "Max damage coefficient of the NovaBomb");

                // Flamethrower

                FlamethrowerDuration = WrapConfigFloat("FlamethrowerDuration",
                    "The duration of the flamethrower");

                FlamethrowerTickFrequency = WrapConfigFloat("FlamethrowerTickFrequency",
                    "The tick frequency of the flamethrower");

                FlamethrowerTickFrequencyScaleWithAttackSpeed = WrapConfigBool(
                    "FlamethrowerTickFrequencyScaleWithAttackSpeed",
                    "If the tick frequency should scale with AttackSpeed. Needs FlamethrowerTickFrequencyScaleCoefficient to be set to work.");

                FlamethrowerTickFrequencyScaleCoefficient = WrapConfigFloat("FlamethrowerTickFrequencyScaleCoefficient",
                    "The coefficient for the AttackSpeed scaling of the Flamethrower. Formula: Coeff * ATKSP * TickFreq");

                FlamethrowerProcCoefficientPerTick = WrapConfigFloat("FlamethrowerProcCoefficientPerTick",
                    "The coefficient for items per proc of the flamethrower.");

                FlamethrowerMaxDistance = WrapConfigFloat("FlamethrowerMaxDistance",
                    "The max distance of the Flamethrower");

                FlamethrowerRadius = WrapConfigFloat("FlamethrowerRadius",
                    "The radius of the Flamethrower");

                FlamethrowerTotalDamageCoefficient = WrapConfigFloat("FlamethrowerTotalDamageCoefficient",
                    "The total damage coefficient for the flamethrower");

                FlamethrowerIgnitePercentChance = WrapConfigFloat("FlamethrowerIgnitePercentChance",
                    "The change to ignite per proc in percent.");
            }


            public override void OverrideGameValues()
            {
                On.RoR2.Run.Awake += (orig, self) =>
                {
                    orig(self);

                    Assembly assembly = self.GetType().Assembly;

                    Type chargeNovaBomb = assembly.GetClass("EntityStates.Mage.Weapon", "ChargeNovabomb");

                    NovaBombMaxChargeDuration.SetDefaultValue(
                        chargeNovaBomb.GetFieldValue<float>("baseChargeDuration"));
                    if (NovaBombMaxChargeDuration.IsNotDefault())
                    {
                        chargeNovaBomb.SetFieldValue("baseChargeDuration", NovaBombMaxChargeDuration.FloatValue);
                    }

                    NovaBombMaxDamageCoefficient.SetDefaultValue(
                        chargeNovaBomb.GetFieldValue<float>("maxDamageCoefficient"));
                    if (NovaBombMaxDamageCoefficient.IsNotDefault())
                    {
                        chargeNovaBomb.SetFieldValue("maxDamageCoefficient", NovaBombMaxDamageCoefficient.FloatValue);
                    }

                    Type flamethrower = assembly.GetClass("EntityStates.Mage.Weapon", "Flamethrower");

                    FlamethrowerDuration.SetDefaultValue(flamethrower.GetFieldValue<float>("baseFlamethrowerDuration"));
                    if (FlamethrowerDuration.IsNotDefault())
                    {
                        flamethrower.SetFieldValue("baseFlamethrowerDuration", FlamethrowerDuration.FloatValue);
                    }

                    VanillaTickFrequency = flamethrower.GetFieldValue<float>("tickFrequency");
                    FlamethrowerTickFrequency.SetDefaultValue(VanillaTickFrequency);
                    if (FlamethrowerTickFrequency.IsNotDefault())
                    {
                        flamethrower.SetFieldValue("tickFrequency", FlamethrowerTickFrequency.FloatValue);
                    }

                    FlamethrowerProcCoefficientPerTick.SetDefaultValue(
                        flamethrower.GetFieldValue<float>("procCoefficientPerTick"));
                    if (FlamethrowerProcCoefficientPerTick.IsNotDefault())
                    {
                        flamethrower.SetFieldValue("procCoefficientPerTick",
                            FlamethrowerProcCoefficientPerTick.FloatValue);
                    }

                    FlamethrowerMaxDistance.SetDefaultValue(flamethrower.GetFieldValue<float>("maxDistance"));
                    if (FlamethrowerMaxDistance.IsNotDefault())
                    {
                        flamethrower.SetFieldValue("maxDistance", FlamethrowerMaxDistance.FloatValue);
                    }

                    FlamethrowerRadius.SetDefaultValue(flamethrower.GetFieldValue<float>("radius"));
                    if (FlamethrowerRadius.IsNotDefault())
                    {
                        flamethrower.SetFieldValue("radius", FlamethrowerRadius.FloatValue);
                    }

                    FlamethrowerTotalDamageCoefficient.SetDefaultValue(
                        flamethrower.GetFieldValue<float>("totalDamageCoefficient"));
                    if (FlamethrowerTotalDamageCoefficient.IsNotDefault())
                    {
                        flamethrower.SetFieldValue("totalDamageCoefficient",
                            FlamethrowerTotalDamageCoefficient.FloatValue);
                    }

                    FlamethrowerIgnitePercentChance.SetDefaultValue(
                        flamethrower.GetFieldValue<float>("ignitePercentChance"));
                    if (FlamethrowerIgnitePercentChance.IsNotDefault())
                    {
                        flamethrower.SetFieldValue("ignitePercentChance", FlamethrowerIgnitePercentChance.FloatValue);
                    }
                };
            }

            public override void WriteNewHooks()
            {
                bool runStockScaling = FireboltAttackSpeedStockScaling.Value &&
                                       FireboltAttackSpeedStockScalingCoefficient.IsNotDefault();

                bool runCooldownScaling = FireboltAttackSpeedCooldownScaling.Value &&
                                          FireboltAttackSpeedCooldownScalingCoefficient.IsNotDefault();

                float cooldownCoeff = FireboltAttackSpeedCooldownScalingCoefficient.FloatValue;
                float stockCoeff = FireboltAttackSpeedStockScalingCoefficient.FloatValue;

                if (runStockScaling || runCooldownScaling)
                {
                    IL.RoR2.CharacterBody.RecalculateStats += il =>
                    {
                        ILCursor c = new ILCursor(il);
                        c.GotoNext(x =>
                            x.MatchRet()
                        );

                        c.Emit(OpCodes.Ldarg_0);
                        c.Emit<CharacterBody>(OpCodes.Ldfld, "skillLocator");
                        c.Emit(OpCodes.Ldloc_S, (byte) 45);
                        c.Emit(OpCodes.Ldloc_S, (byte) 42);
                        c.EmitDelegate<Action<SkillLocator, float, float>>(
                            (skillLocator, cooldownScale, attackSpeed) =>
                            {
                                GenericSkill primary = skillLocator != null ? skillLocator.primary : null;
                                if (primary != null && primary.skillName == "FireFirebolt")
                                {
                                    if (runCooldownScaling)
                                    {
                                        primary.cooldownScale = cooldownScale * (1 / (cooldownCoeff * attackSpeed));
                                    }

                                    if (runStockScaling)
                                    {
                                        primary.SetBonusStockFromBody((int) (attackSpeed * stockCoeff));
                                    }
                                }
                            });
                    };
                }

                if (FlamethrowerTickFrequencyScaleWithAttackSpeed.Value &&
                    FlamethrowerTickFrequencyScaleCoefficient.IsNotDefault())
                {
                    On.EntityStates.Mage.Weapon.Flamethrower.OnEnter += (orig, self) =>
                    {
                        Type flamethrowerType = self.GetType();

                        GameObject go = typeof(EntityState)
                            .GetProperty("gameObject", BindingFlags.NonPublic | BindingFlags.Instance)
                            ?.GetValue(self) as GameObject;

                        CharacterBody body = go.GetComponent<CharacterBody>();

                        float val = body.attackSpeed *
                                    FlamethrowerTickFrequencyScaleCoefficient.FloatValue *
                                    (FlamethrowerTickFrequency.IsNotDefault()
                                        ? FlamethrowerTickFrequency.FloatValue
                                        : VanillaTickFrequency);

                        flamethrowerType.SetFieldValue("tickFrequency",
                            val
                        );

                        orig(self);
                    };
                }
            }
        }
    }
}