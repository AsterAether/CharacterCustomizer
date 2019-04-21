using System;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Configuration;
using EntityStates;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;
using AetherLib.Util;
using AetherLib.Util.Config;
using AetherLib.Util.Reflection;
using EntityStates.Commando;
using R2API;

namespace CharacterCustomizer.CustomSurvivors
{
    namespace Commando
    {
        public class CustomCommando : CustomSurvivor
        {
            public CustomCommando() : base("Commando")
            {
            }

            public ValueConfigWrapper<string> PistolDamageCoefficient;

            public ValueConfigWrapper<string> PistolBaseDuration;

            public ValueConfigWrapper<string> LaserDamageCoefficient;

            public ValueConfigWrapper<string> LaserCooldown;

            public ConfigWrapper<bool> DashResetsSecondCooldown;

            public ValueConfigWrapper<int> DashStockCount;

            public ValueConfigWrapper<string> DashCooldown;

            public ConfigWrapper<bool> PistolHitLowerBarrageCooldown;
            public ValueConfigWrapper<string> PistolHitLowerBarrageCooldownPercent;

            public ValueConfigWrapper<string> BarrageCooldown;


            public ConfigWrapper<bool> BarrageScalesWithAttackSpeed;

            public ValueConfigWrapper<string> BarrageScaleModifier;

            public ValueConfigWrapper<int> BarrageBaseShotAmount;

            public int VanillaBarrageBaseShotAmount;

            public ValueConfigWrapper<string> BarrageBaseDurationBetweenShots;

            public float VanillaBarrageBaseDurationBeetweenShots;

            public override void InitConfigValues()
            {
                PistolDamageCoefficient = WrapConfigFloat("PistolDamageCoefficient",
                    "Damage coefficient for the pistol, in percent.");

                LaserDamageCoefficient = WrapConfigFloat("LaserDamageCoefficient",
                    "Damage coefficient for the secondary laser, in percent.");

                PistolBaseDuration =
                    WrapConfigFloat("PistolBaseDuration",
                        "Base duration for the pistol shot, in percent. (Attack Speed)");


                DashResetsSecondCooldown =
                    WrapConfigBool("DashResetsSecondCooldown",
                        "If the dash should reset the cooldown of the second ability.");


                DashStockCount = WrapConfigInt("DashStockCount", "How many stocks the dash ability has.");

                DashCooldown = WrapConfigFloat("DashCooldown", "Cooldown of the dash, in seconds");

                LaserCooldown = WrapConfigFloat("LaserCooldown", "Cooldown of the secondary laser, in seconds");

                PistolHitLowerBarrageCooldownPercent = WrapConfigFloat("PistolHitLowerBarrageCooldownPercent",
                    "The amount in percent that the current cooldown of the Barrage Skill should be lowered by. Needs to have PistolHitLowerBarrageCooldownPercent set.");


                PistolHitLowerBarrageCooldown =
                    WrapConfigBool("PistolHitLowerBarrageCooldown",
                        "If the pistol hit should lower the Barrage Skill cooldown. Needs to have PistolHitLowerBarrageCooldownPercent set to work");


                BarrageCooldown = WrapConfigFloat("BarrageCooldown", "Cooldown of the Barrage Skill, in seconds");


                BarrageScalesWithAttackSpeed = WrapConfigBool("BarrageScalesWithAttackSpeed",
                    "If the barrage bullet count should scale with attackspeed. Idea by @Twyla. Needs BarrageScaleModifier to be set.");


                BarrageScaleModifier = WrapConfigFloat("BarrageScaleCoefficient",
                    "Coefficient for the AttackSpeed scale of Barrage bullet count, in percent. Formula: BCount * ATKSP * Coeff");


                BarrageBaseShotAmount =
                    WrapConfigInt("BarrageBaseShotAmount", "How many shots the Barrage skill should fire");


                BarrageBaseDurationBetweenShots =
                    WrapConfigFloat("BarrageBaseDurationBetweenShots",
                        "Base duration between shots in the Barrage skill.");
            }

            public override void OverrideGameValues()
            {
                On.RoR2.Run.Awake += (orig, self) =>
                {
                    orig(self);
                    Assembly assembly = self.GetType().Assembly;

                    Type firePistol = assembly.GetClass("EntityStates.Commando.CommandoWeapon", "FirePistol2");

                    
                    PistolDamageCoefficient.SetDefaultValue(firePistol.GetFieldValue<float>("damageCoefficient"));
                    if (PistolDamageCoefficient.IsNotDefault())
                    {
                        firePistol.SetFieldValue("damageCoefficient", PistolDamageCoefficient.FloatValue);
                    }

                    PistolBaseDuration.SetDefaultValue(firePistol.GetFieldValue<float>("baseDuration"));
                    if (PistolBaseDuration.IsNotDefault())
                    {
                        firePistol.SetFieldValue("baseDuration", PistolBaseDuration.FloatValue);
                    }

                    Type fireLaser = assembly.GetClass("EntityStates.Commando.CommandoWeapon", "FireFMJ");

                    LaserDamageCoefficient.SetDefaultValue(fireLaser.GetFieldValue<float>("damageCoefficient"));
                    if (LaserDamageCoefficient.IsNotDefault())
                    {
                        fireLaser.SetFieldValue("damageCoefficient", LaserDamageCoefficient.FloatValue);
                    }

                    Type fireBarr = assembly.GetClass("EntityStates.Commando.CommandoWeapon", "FireBarrage");

                    VanillaBarrageBaseShotAmount =
                        fireBarr.GetFieldValue<int>("bulletCount");
                    
                    BarrageBaseShotAmount.SetDefaultValue(VanillaBarrageBaseShotAmount);

                    VanillaBarrageBaseDurationBeetweenShots = fireBarr.GetFieldValue<float>("baseDurationBetweenShots");

                    BarrageBaseDurationBetweenShots.SetDefaultValue(VanillaBarrageBaseDurationBeetweenShots);
                    
                    if (BarrageBaseDurationBetweenShots.IsNotDefault())
                    {
                        fireBarr.SetFieldValue("baseDurationBetweenShots", BarrageBaseDurationBetweenShots.FloatValue);
                    }

                    BarrageBaseShotAmount.RunIfNotDefault(count => { fireBarr.SetFieldValue("bulletCount", count); });
                };

                SurvivorAPI.SurvivorCatalogReady += (sender, args) =>
                {
                    SurvivorDef commando =
                        SurvivorAPI.SurvivorDefinitions.First(def => def.survivorIndex == SurvivorIndex.Commando);
                    GenericSkill[] skills = commando.bodyPrefab.GetComponents<GenericSkill>();
                    foreach (GenericSkill genericSkill in skills)
                    {
                        switch (genericSkill.skillName)
                        {
                            case "Barrage":
                                BarrageCooldown.SetDefaultValue(genericSkill.baseRechargeInterval);
                                if (BarrageCooldown.IsNotDefault())
                                {
                                    genericSkill.baseRechargeInterval = BarrageCooldown.FloatValue;
                                }

                                break;
                            case "FirePistol":
                                break;
                            case "FireFMJ":
                                LaserCooldown.SetDefaultValue(genericSkill.baseRechargeInterval);
                                if (LaserCooldown.IsNotDefault())
                                {
                                    genericSkill.baseRechargeInterval = LaserCooldown.FloatValue;
                                }

                                break;
                            case "Roll":
                                DashStockCount.SetDefaultValue(genericSkill.baseMaxStock);
                                DashStockCount.RunIfNotDefault(stock =>
                                {
                                    
                                    genericSkill.stockToConsume = 1;
                                    genericSkill.requiredStock = 1;
                                    genericSkill.rechargeStock = 1;
                                    genericSkill.baseMaxStock = stock;
                                });
                                DashCooldown.SetDefaultValue(genericSkill.baseRechargeInterval);
                                if (DashCooldown.IsNotDefault())
                                {
                                    genericSkill.baseRechargeInterval = DashCooldown.FloatValue;
                                }

                                break;
                        }
                    }

                    SurvivorAPI.ReconstructSurvivors();
                };
            }

            public override void WriteNewHooks()
            {
                if (BarrageScalesWithAttackSpeed.Value && BarrageScaleModifier.IsNotDefault())
                {
                    On.EntityStates.Commando.CommandoWeapon.FireBarrage.FixedUpdate += (orig, self) =>
                    {
                        Assembly assembly = self.GetType().Assembly;
                        Type fireBarr = assembly.GetClass("EntityStates.Commando.CommandoWeapon", "FireBarrage");
                        FieldInfo attackSpeed = typeof(BaseState).GetField("attackSpeedStat",
                            BindingFlags.NonPublic | BindingFlags.Instance);

                        FieldInfo durationBetweenShots = fireBarr.GetField("durationBetweenShots",
                            BindingFlags.NonPublic | BindingFlags.Instance);

                        float attackSpeedF = (float) attackSpeed?.GetValue(self);

                        int baseShot = BarrageBaseShotAmount.IsDefault()
                            ? VanillaBarrageBaseShotAmount
                            : BarrageBaseShotAmount.Value;

                        durationBetweenShots?.SetValue(self,
                            (BarrageBaseDurationBetweenShots.IsDefault()
                                ? VanillaBarrageBaseDurationBeetweenShots
                                : BarrageBaseDurationBetweenShots.FloatValue) / attackSpeedF /
                            BarrageScaleModifier.FloatValue);

                        fireBarr.SetFieldValue("bulletCount",
                            (int) (attackSpeedF * BarrageScaleModifier.FloatValue * baseShot));
                        orig(self);
                    };
                }

                if (DashResetsSecondCooldown.Value)
                {
                    On.EntityStates.Commando.DodgeState.OnEnter += (orig, self) =>
                    {
                        orig(self);

                        Assembly assembly = self.GetType().Assembly;

                        Type entityState = assembly.GetClass("EntityStates", "EntityState");

                        SkillLocator locator = (SkillLocator) entityState
                            .GetProperty("skillLocator", BindingFlags.NonPublic | BindingFlags.Instance)
                            ?.GetValue(self, null);

                        GenericSkill skill2 = locator.GetSkill(SkillSlot.Secondary);
                        skill2.Reset();
                    };
                }


                if (PistolHitLowerBarrageCooldown.Value && PistolHitLowerBarrageCooldownPercent.IsNotDefault())
                {
                    Type gsType = typeof(GenericSkill);
                    FieldInfo rechargeStopwatch = gsType.GetField("rechargeStopwatch",
                        BindingFlags.NonPublic | BindingFlags.Instance);
                    FieldInfo finalRechargeInterval = gsType.GetField("finalRechargeInterval",
                        BindingFlags.NonPublic | BindingFlags.Instance);

                    IL.EntityStates.Commando.CommandoWeapon.FirePistol2.FireBullet += il =>
                    {
                        ILCursor c = new ILCursor(il);

                        c.GotoNext(x => x.MatchCallvirt(typeof(RoR2.BulletAttack).FullName, "Fire"));
                        c.EmitDelegate<Func<BulletAttack, BulletAttack>>((BulletAttack ba) =>
                        {
                            ba.hitCallback = (ref BulletAttack.BulletHit info) =>
                            {
                                bool result = ba.DefaultHitCallback(ref info);
                                if (info.entityObject?.GetComponent<HealthComponent>())
                                {
                                    SkillLocator skillLocator = ba.owner.GetComponent<SkillLocator>();
                                    GenericSkill special = skillLocator.special;
                                    rechargeStopwatch.SetValue(special,
                                        (float) rechargeStopwatch.GetValue(special) +
                                        (float) finalRechargeInterval.GetValue(special) *
                                        PistolHitLowerBarrageCooldownPercent.FloatValue);
                                }

                                return result;
                            };
                            return ba;
                        });
                    };
                }
            }
        }
    }
}