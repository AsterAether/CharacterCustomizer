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
                        "If the charge count of the FireBolt Skill should scale with AttackSpeed.");


                FireboltAttackSpeedStockScalingCoefficent =
                    WrapConfigFloat("FireboltAttackSpeedStockScalingCoefficent",
                        "Coeefiecent for charge AttackSpeed scaling, in percent.");

                FireboltAttackSpeedCooldownScaling = WrapConfigBool("FireboltAttackSpeedCooldownScaling",
                    "If the cooldown of the Firebolt Skill should scale with AttackSpeed.");


                FireboltAttackSpeedCooldownScalingCoefficent = WrapConfigFloat(
                    "FireboltAttackSpeedCooldownScalingCoefficent",
                    "Coefficient for cooldown AttackSpeed scaling, in percent.");
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

                FieldInfo skillLocatorField =
                    typeof(CharacterBody).GetField("skillLocator", BindingFlags.NonPublic | BindingFlags.Instance);


                if (runStockScaling || runCooldownScaling)
                {
                    IL.RoR2.CharacterBody.RecalculateStats += il =>
                    {
                        ILCursor c = new ILCursor(il);
                        c.GotoNext(x => x.MatchRet());

                        c.Emit(OpCodes.Ldloc_S, (byte) 45);
                        c.Emit(OpCodes.Ldloc_S, (byte) 42);
                        c.Emit(OpCodes.Ldarg_0);
                        c.EmitDelegate<Action<float, float, CharacterBody>>(
                            (cooldownScale, attackSpeed, characterBody) =>
                            {
                                SkillLocator skillLocator = (SkillLocator) skillLocatorField?.GetValue(characterBody);
                                GenericSkill primary = skillLocator != null ? skillLocator.primary : null;
                                if (primary != null && primary.skillName == "FireFirebolt")
                                {
                                    if (runStockScaling)
                                    {
                                        primary.SetBonusStockFromBody((int) (attackSpeed * stockCoeff));
                                    }

                                    if (runCooldownScaling)
                                    {
                                        primary.cooldownScale = cooldownScale * (1 / (cooldownCoeff * attackSpeed));
                                    }
                                }
                            });
                    };
                }
            }
        }
    }
}