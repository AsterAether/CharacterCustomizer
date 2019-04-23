using System;
using System.Collections.Generic;
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

            public List<IFieldChanger> FireboltFields;

            public FieldConfigWrapper<string> NovaBombMaxChargeDuration;

            public FieldConfigWrapper<string> NovaBombMaxDamageCoefficient;

            public List<IFieldChanger> ChargeNovaBombFields;

            public FieldConfigWrapper<string> FlamethrowerDuration;

            public FieldConfigWrapper<string> FlamethrowerTickFrequency;

            public ConfigWrapper<bool> FlamethrowerTickFrequencyScaleWithAttackSpeed;

            public float VanillaFlamethrowerTickFrequency;

            public ValueConfigWrapper<string> FlamethrowerTickFrequencyScaleCoefficient;

            public FieldConfigWrapper<string> FlamethrowerProcCoefficientPerTick;

            public FieldConfigWrapper<string> FlamethrowerMaxDistance;

            public FieldConfigWrapper<string> FlamethrowerRadius;

            public FieldConfigWrapper<string> FlamethrowerTotalDamageCoefficient;

            public FieldConfigWrapper<string> FlamethrowerIgnitePercentChance;

            public ConfigWrapper<bool> FlamethrowerDurationScaleDownWithAttackSpeed;

            public ValueConfigWrapper<string> FlamethrowerDurationScaleCoefficient;

            public ValueConfigWrapper<string> FlamethrowerMinimalDuration;

            public float VanillaFlamethrowerDuration;

            public List<IFieldChanger> FlamethrowerFields;


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
                    WrapConfigStandardBool("FireboltAttackSpeedStockScaling",
                        "If the charge count of the FireBolt Skill should scale with AttackSpeed. Needs to have FireboltAttackSpeedStockScalingCoefficent set to work.");


                FireboltAttackSpeedStockScalingCoefficient =
                    WrapConfigFloat("FireboltAttackSpeedStockScalingCoefficient",
                        "Coefficient for charge AttackSpeed scaling, in percent. Formula: Stock + Stock * (ATKSP - 1) * Coeff.");

                FireboltAttackSpeedCooldownScaling = WrapConfigStandardBool("FireboltAttackSpeedCooldownScaling",
                    "If the cooldown of the Firebolt Skill should scale with AttackSpeed. Needs to have FireboltAttackSpeedCooldownScalingCoefficent set to work.");


                FireboltAttackSpeedCooldownScalingCoefficient = WrapConfigFloat(
                    "FireboltAttackSpeedCooldownScalingCoefficient",
                    "Coefficient for cooldown AttackSpeed scaling, in percent. Formula: BaseCooldown * (1 / (1 + (ATKSP-1) * Coeff)) .");

                // NovaBomb

                NovaBombMaxChargeDuration = new FieldConfigWrapper<string>(WrapConfigFloat("NovaBombBaseChargeDuration",
                    "Base max charging duration of the NovaBomb"), "baseChargeDuration", true);

                NovaBombMaxDamageCoefficient = new FieldConfigWrapper<string>(WrapConfigFloat(
                        "NovaBombMaxDamageCoefficient",
                        "Max damage coefficient of the NovaBomb"), "maxDamageCoefficient",
                    true);

                ChargeNovaBombFields = new List<IFieldChanger>
                {
                    NovaBombMaxChargeDuration,
                    NovaBombMaxDamageCoefficient
                };

                // Flamethrower

                FlamethrowerProcCoefficientPerTick = new FieldConfigWrapper<string>(WrapConfigFloat(
                    "FlamethrowerProcCoefficientPerTick",
                    "The coefficient for items per proc of the flamethrower."), "procCoefficientPerTick", true);

                FlamethrowerMaxDistance = new FieldConfigWrapper<string>(WrapConfigFloat("FlamethrowerMaxDistance",
                    "The max distance of the Flamethrower"), "maxDistance", true);

                FlamethrowerRadius = new FieldConfigWrapper<string>(WrapConfigFloat("FlamethrowerRadius",
                    "The radius of the Flamethrower"), "radius", true);

                FlamethrowerTotalDamageCoefficient = new FieldConfigWrapper<string>(WrapConfigFloat(
                    "FlamethrowerTotalDamageCoefficient",
                    "The total damage coefficient for the flamethrower"), "totalDamageCoefficient", true);

                FlamethrowerIgnitePercentChance = new FieldConfigWrapper<string>(WrapConfigFloat(
                    "FlamethrowerIgnitePercentChance",
                    "The change to ignite per proc in percent."), "ignitePercentChance", true);

                FlamethrowerFields = new List<IFieldChanger>
                {
                    FlamethrowerProcCoefficientPerTick,
                    FlamethrowerMaxDistance,
                    FlamethrowerRadius,
                    FlamethrowerTotalDamageCoefficient,
                    FlamethrowerIgnitePercentChance
                };

                FlamethrowerDuration = new FieldConfigWrapper<string>(WrapConfigFloat("FlamethrowerDuration",
                    "The duration of the flamethrower"), "baseFlamethrowerDuration", true);

                FlamethrowerTickFrequency = new FieldConfigWrapper<string>(WrapConfigFloat("FlamethrowerTickFrequency",
                    "The tick frequency of the flamethrower"), "tickFrequency", true);

                FlamethrowerTickFrequencyScaleWithAttackSpeed = WrapConfigStandardBool(
                    "FlamethrowerTickFrequencyScaleWithAttackSpeed",
                    "If the tick frequency should scale with AttackSpeed. Needs FlamethrowerTickFrequencyScaleCoefficient to be set to work.");

                FlamethrowerTickFrequencyScaleCoefficient = WrapConfigFloat("FlamethrowerTickFrequencyScaleCoefficient",
                    "The coefficient for the AttackSpeed scaling of the Flamethrower. Formula: TickFreq + Coeff * (ATKSP - 1) * TickFreq");

                FlamethrowerDurationScaleDownWithAttackSpeed =
                    WrapConfigStandardBool("FlamethrowerDurationScaleDownWithAttackSpeed",
                        "If the flame thrower duration should get shorter with more attack speed. Needs FlamethrowerDurationScaleCoefficient to be set.");

                FlamethrowerDurationScaleCoefficient = WrapConfigFloat("FlamethrowerDurationScaleCoefficient",
                    "The coefficient for flame thrower scaling. Formula: Duration - Coeff * (ATKSP - 1) * Duration. Minimum of FlamethrowerMinimalDuration seconds.");

                FlamethrowerMinimalDuration = WrapConfigFloat("FlamethrowerMinimalDuration",
                    "The minimal duration of the flamethrower",
                    1f);
            }


            public override void OverrideGameValues()
            {
                On.RoR2.Run.Awake += (orig, self) =>
                {
                    orig(self);

                    Assembly assembly = self.GetType().Assembly;

                    Type chargeNovaBomb = assembly.GetClass("EntityStates.Mage.Weapon", "ChargeNovabomb");

                    ChargeNovaBombFields.ForEach(changer => changer.Apply(chargeNovaBomb));

                    Type flamethrower = assembly.GetClass("EntityStates.Mage.Weapon", "Flamethrower");

                    FlamethrowerFields.ForEach(changer => changer.Apply(flamethrower));

                    VanillaFlamethrowerTickFrequency = FlamethrowerTickFrequency.GetValue<float>(flamethrower);
                    VanillaFlamethrowerDuration = FlamethrowerDuration.GetValue<float>(flamethrower);
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
                                        float val = cooldownScale *
                                                    (1 / (1 + cooldownCoeff * (attackSpeed - 1)));
                                        primary.cooldownScale = val;
                                    }

                                    if (runStockScaling)
                                    {
                                        primary.SetBonusStockFromBody(
                                            (int) ((attackSpeed - 1) * stockCoeff * primary.maxStock));
                                    }
                                }
                            });
                    };
                }


                On.EntityStates.Mage.Weapon.Flamethrower.OnEnter += (orig, self) =>
                {
                    Type flamethrowerType = self.GetType();

                    GameObject go = typeof(EntityState)
                        .GetProperty("gameObject", BindingFlags.NonPublic | BindingFlags.Instance)
                        ?.GetValue(self) as GameObject;

                    CharacterBody body = go.GetComponent<CharacterBody>();

                    if (FlamethrowerTickFrequencyScaleWithAttackSpeed.Value &&
                        FlamethrowerTickFrequencyScaleCoefficient.IsNotDefault())
                    {
                        float baseVal = FlamethrowerTickFrequency.ValueConfigWrapper.IsNotDefault()
                            ? FlamethrowerTickFrequency.ValueConfigWrapper.FloatValue
                            : VanillaFlamethrowerTickFrequency;

                        float val = baseVal - (body.attackSpeed - 1) *
                                    FlamethrowerTickFrequencyScaleCoefficient.FloatValue * baseVal
                            ;

                        flamethrowerType.SetFieldValue("tickFrequency",
                            val
                        );
                    }

                    if (FlamethrowerDurationScaleDownWithAttackSpeed.Value &&
                        FlamethrowerDurationScaleCoefficient.IsNotDefault())
                    {
                        float baseVal = FlamethrowerDuration.ValueConfigWrapper.IsNotDefault()
                            ? FlamethrowerDuration.ValueConfigWrapper.FloatValue
                            : VanillaFlamethrowerDuration;

                        float val = baseVal - (body.attackSpeed - 1) *
                                    FlamethrowerDurationScaleCoefficient.FloatValue *
                                    baseVal;

                        if (val < FlamethrowerMinimalDuration.FloatValue)
                        {
                            val = FlamethrowerMinimalDuration.FloatValue;
                        }

                        flamethrowerType.SetFieldValue("baseFlamethrowerDuration",
                            val
                        );
                    }


                    orig(self);
                };
            }
        }
    }
}