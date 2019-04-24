using System;
using System.Collections.Generic;
using System.Reflection;
using AetherLib.Util.Config;
using AetherLib.Util.Reflection;
using BepInEx.Configuration;
using EntityStates;
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace CharacterCustomizer.CustomSurvivors.Survivors
{
    namespace Commando
    {
        public class CustomCommando : CustomSurvivor
        {
            public CustomCommando() : base(SurvivorIndex.Commando, "Commando",
                "FirePistol",
                "FireFMJ",
                "Roll",
                "Barrage")
            {
            }

            public ConfigWrapper<bool> PistolHitLowerBarrageCooldown;

            public ValueConfigWrapper<string> PistolHitLowerBarrageCooldownPercent;

            public FieldConfigWrapper<string> PistolDamageCoefficient;

            public FieldConfigWrapper<string> PistolBaseDuration;

            public List<IFieldChanger> PistolFields;

            public FieldConfigWrapper<string> LaserDamageCoefficient;

            public List<IFieldChanger> LaserFields;

            public ConfigWrapper<bool> DashResetsSecondCooldown;

            public ConfigWrapper<bool> DashInvulnerability;

            public ValueConfigWrapper<string> DashInvulnerabilityTimer;

            public List<IFieldChanger> DashFields;

            public ConfigWrapper<bool> BarrageScalesWithAttackSpeed;

            public ValueConfigWrapper<string> BarrageScaleModifier;

            public int VanillaBarrageBaseShotAmount;

            public float VanillaBarrageBaseDurationBetweenShots;

            public FieldConfigWrapper<string> BarrageBaseDurationBetweenShots;

            public FieldConfigWrapper<int> BarrageBaseShotAmount;

            public List<IFieldChanger> BarrageFields;

            public override void InitConfigValues()
            {
                // Pistol

                PistolDamageCoefficient = new FieldConfigWrapper<string>(WrapConfigFloat("PistolDamageCoefficient",
                    "Damage coefficient for the pistol, in percent."), "damageCoefficient", true);

                PistolBaseDuration =
                    new FieldConfigWrapper<string>(WrapConfigFloat("PistolBaseDuration",
                        "Base duration for the pistol shot, in percent. (Attack Speed)"), "baseDuration", true);

                PistolFields = new List<IFieldChanger> {PistolBaseDuration, PistolDamageCoefficient};

                PistolHitLowerBarrageCooldownPercent = WrapConfigFloat("PistolHitLowerBarrageCooldownPercent",
                    "The amount in percent that the current cooldown of the Barrage Skill should be lowered by. Needs to have PistolHitLowerBarrageCooldownPercent set.");


                PistolHitLowerBarrageCooldown =
                    WrapConfigStandardBool("PistolHitLowerBarrageCooldown",
                        "If the pistol hit should lower the Barrage Skill cooldown. Needs to have PistolHitLowerBarrageCooldownPercent set to work");

                // Laser

                LaserDamageCoefficient = new FieldConfigWrapper<string>(WrapConfigFloat("LaserDamageCoefficient",
                    "Damage coefficient for the secondary laser, in percent."), "damageCoefficient", true);

                LaserFields = new List<IFieldChanger> {LaserDamageCoefficient};

                // Dash

                DashResetsSecondCooldown =
                    WrapConfigStandardBool("DashResetsSecondCooldown",
                        "If the dash should reset the cooldown of the second ability.");

                DashInvulnerability = WrapConfigStandardBool("DashInvulnerability",
                    "If Commando should be invulnerable while dashing.");

                DashInvulnerabilityTimer = WrapConfigFloat("DashInvulnerabilityTimer",
                    "How long Commando should be invincible for when dashing. Only active when DashInvulnerability is on. 0 = For the whole dash.");


                // Barrage

                BarrageScalesWithAttackSpeed = WrapConfigStandardBool("BarrageScalesWithAttackSpeed",
                    "If the barrage bullet count should scale with attack speed. Idea by @Twyla. Needs BarrageScaleModifier to be set.");


                BarrageScaleModifier = WrapConfigFloat("BarrageScaleCoefficient",
                    "Coefficient for the AttackSpeed scale of Barrage bullet count, in percent. Formula: BCount + BCount * (ATKSP - 1) * Coeff");


                BarrageBaseShotAmount =
                    new FieldConfigWrapper<int>(
                        WrapConfigInt("BarrageBaseShotAmount", "How many shots the Barrage skill should fire"),
                        "bulletCount", true);


                BarrageBaseDurationBetweenShots =
                    new FieldConfigWrapper<string>(WrapConfigFloat("BarrageBaseDurationBetweenShots",
                        "Base duration between shots in the Barrage skill."), "baseDurationBetweenShots", true);

                BarrageFields = new List<IFieldChanger> {BarrageBaseShotAmount, BarrageBaseDurationBetweenShots};
            }

            public override void OverrideGameValues()
            {
                On.RoR2.RoR2Application.Start += (orig, self) =>
                {
                    orig(self);
                    Assembly assembly = self.GetType().Assembly;

                    Type firePistol = assembly.GetClass("EntityStates.Commando.CommandoWeapon", "FirePistol2");

                    PistolFields.ForEach(changer => changer.Apply(firePistol));

                    Type fireLaser = assembly.GetClass("EntityStates.Commando.CommandoWeapon", "FireFMJ");

                    LaserFields.ForEach(changer => changer.Apply(fireLaser));

                    Type fireBarr = assembly.GetClass("EntityStates.Commando.CommandoWeapon", "FireBarrage");

                    VanillaBarrageBaseShotAmount =
                        BarrageBaseShotAmount.GetValue<int>(fireBarr);

                    VanillaBarrageBaseDurationBetweenShots = BarrageBaseDurationBetweenShots.GetValue<float>(fireBarr);

                    BarrageFields.ForEach(changer => changer.Apply(fireBarr));
                };
            }

            public override void WriteNewHooks()
            {
                if (BarrageScalesWithAttackSpeed.Value && BarrageScaleModifier.IsNotDefault())
                {
                    On.EntityStates.Commando.CommandoWeapon.FireBarrage.OnEnter += (orig, self) =>
                    {
                        orig(self);

                        Assembly assembly = self.GetType().Assembly;

                        Type fireBarr = assembly.GetClass("EntityStates.Commando.CommandoWeapon", "FireBarrage");
                        FieldInfo attackSpeed = typeof(BaseState).GetField("attackSpeedStat",
                            BindingFlags.NonPublic | BindingFlags.Instance);

                        FieldInfo durationBetweenShots = fireBarr.GetField("durationBetweenShots",
                            BindingFlags.NonPublic | BindingFlags.Instance);

                        float attackSpeedF = (float) attackSpeed?.GetValue(self);

                        int baseShot = BarrageBaseShotAmount.ValueConfigWrapper.IsDefault()
                            ? VanillaBarrageBaseShotAmount
                            : BarrageBaseShotAmount.ValueConfigWrapper.Value;

                        durationBetweenShots?.SetValue(self,
                            (BarrageBaseDurationBetweenShots.ValueConfigWrapper.IsDefault()
                                ? VanillaBarrageBaseDurationBetweenShots
                                : BarrageBaseDurationBetweenShots.ValueConfigWrapper.FloatValue) / attackSpeedF /
                            BarrageScaleModifier.FloatValue);

                        fireBarr.SetFieldValue("bulletCount",
                            baseShot + (int) ((attackSpeedF - 1) * BarrageScaleModifier.FloatValue * baseShot));
                    };
                }


                On.EntityStates.Commando.DodgeState.OnEnter += (orig, self) =>
                {
                    orig(self);

                    if (DashResetsSecondCooldown.Value)
                    {
                        Assembly assembly = self.GetType().Assembly;

                        Type entityState = assembly.GetClass("EntityStates", "EntityState");

                        SkillLocator locator = (SkillLocator) entityState
                            .GetProperty("skillLocator", BindingFlags.NonPublic | BindingFlags.Instance)
                            ?.GetValue(self, null);

                        GenericSkill skill2 = locator.GetSkill(SkillSlot.Secondary);
                        skill2.Reset();
                    }

                    if (DashInvulnerability.Value)
                    {
                        if (DashInvulnerabilityTimer.IsDefault())
                        {
                            Transform transform = self.InvokeMethod<Transform>("GetModelTransform");

                            HurtBoxGroup hurtBoxGroup = transform.GetComponent<HurtBoxGroup>();
                            ++hurtBoxGroup.hurtBoxesDeactivatorCounter;
                        }
                        else
                        {
                            self.outer.commonComponents.characterBody.AddTimedBuff(BuffIndex.HiddenInvincibility,
                                DashInvulnerabilityTimer.FloatValue);
                        }
                    }
                };

                On.EntityStates.Commando.DodgeState.OnExit += (orig, self) =>
                {
                    if (DashInvulnerability.Value && DashInvulnerabilityTimer.IsDefault())
                    {
                        Transform transform = self.InvokeMethod<Transform>("GetModelTransform");

                        HurtBoxGroup hurtBoxGroup = transform.GetComponent<HurtBoxGroup>();
                        --hurtBoxGroup.hurtBoxesDeactivatorCounter;
                    }

                    orig(self);
                };


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