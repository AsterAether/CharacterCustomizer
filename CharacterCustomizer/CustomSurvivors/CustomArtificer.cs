using System;
using System.Linq;
using System.Reflection;
using AetherLib.Util;
using AetherLib.Util.Config;
using BepInEx;
using BepInEx.Configuration;
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
            public ValueConfigWrapper<int> FireboltChargeCount;

            public ValueConfigWrapper<string> FireboltCooldown;

            public ConfigWrapper<bool> FireboltAttackSpeedStockScaling;

            public ValueConfigWrapper<string> FireboltAttackSpeedStockScalingCoefficent;

            public ConfigWrapper<bool> FireboltAttackSpeedCooldownScaling;

            public ValueConfigWrapper<string> FireboltAttackSpeedCooldownScalingCoefficent;


            public CustomArtificer() : base("Artificer")
            {
            }

            public override void InitConfigValues()
            {
                FireboltChargeCount =
                    WrapConfigInt("FireBoltChargeCount", "Charge count of the Firebolt skill.");

                FireboltCooldown =
                    WrapConfigFloat("FireboltCooldown", "Cooldown of the Firebolt skill, in seconds");

                FireboltAttackSpeedStockScaling =
                    WrapConfigBool("FireboltAttackSpeedStockScaling",
                        "If the charge count of the FireBolt Skill should scale with AttackSpeed. Needs to have FireboltAttackSpeedStockScalingCoefficent set to work.");


                FireboltAttackSpeedStockScalingCoefficent =
                    WrapConfigFloat("FireboltAttackSpeedStockScalingCoefficent",
                        "Coefficient for charge AttackSpeed scaling, in percent. Formula: Stock * ATKSP * Coeff.");

                FireboltAttackSpeedCooldownScaling = WrapConfigBool("FireboltAttackSpeedCooldownScaling",
                    "If the cooldown of the Firebolt Skill should scale with AttackSpeed. Needs to have FireboltAttackSpeedCooldownScalingCoefficent set to work.");


                FireboltAttackSpeedCooldownScalingCoefficent = WrapConfigFloat(
                    "FireboltAttackSpeedCooldownScalingCoefficent",
                    "Coefficient for cooldown AttackSpeed scaling, in percent. Formula: BaseCooldown * (1 / (ATKSP * Coeff)).");
            }


            public override void OverrideGameValues()
            {
                SurvivorAPI.SurvivorCatalogReady += (sender, args) =>
                {
                    SurvivorDef mage =
                        SurvivorAPI.SurvivorDefinitions.First(def => def.survivorIndex == SurvivorIndex.Mage);
                    GenericSkill[] skills = mage.bodyPrefab.GetComponents<GenericSkill>();
                    foreach (GenericSkill genericSkill in skills)
                    {
                        switch (genericSkill.skillName)
                        {
                            case "FireFirebolt":

                                FireboltCooldown.SetDefaultValue(genericSkill.baseRechargeInterval);

                                if (FireboltCooldown.IsNotDefault())
                                {
                                    genericSkill.baseRechargeInterval = FireboltCooldown.FloatValue;
                                }

                                FireboltChargeCount.SetDefaultValue(genericSkill.baseMaxStock);

                                if (FireboltChargeCount.IsNotDefault())
                                {
                                    genericSkill.baseMaxStock = FireboltChargeCount.Value;
                                }

                                break;
                            case "NovaBomb":
                                break;
                            case "Wall":
                                break;
                            case "Flamethrower":
                                break;
                        }
                    }

                    SurvivorAPI.ReconstructSurvivors();
                };
            }

            public override void WriteNewHooks()
            {
                bool runStockScaling = FireboltAttackSpeedStockScaling.Value &&
                                       FireboltAttackSpeedStockScalingCoefficent.IsNotDefault();

                bool runCooldownScaling = FireboltAttackSpeedCooldownScaling.Value &&
                                          FireboltAttackSpeedCooldownScalingCoefficent.IsNotDefault();

                float cooldownCoeff = FireboltAttackSpeedCooldownScalingCoefficent.FloatValue;
                float stockCoeff = FireboltAttackSpeedStockScalingCoefficent.FloatValue;

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
            }
        }
    }
}